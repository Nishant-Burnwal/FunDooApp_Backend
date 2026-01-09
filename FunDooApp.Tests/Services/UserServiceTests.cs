using BusinessLogicLayer.Interface;
using BusinessLogicLayer.Service;
using DatabaseLayer.Exceptions;
using DatabaseLayer.Interface;
using Microsoft.Extensions.Logging;
using ModelLayer.DTOs;
using ModelLayer.DTOs.Email;
using ModelLayer.DTOs.User;
using ModelLayer.Entity;
using Moq;
using NUnit.Framework;

namespace FunDooApp.Tests.Services
{
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepoMock;
        private Mock<ITokenService> _tokenServiceMock;
        private Mock<IMessagePublisher> _messagePublisherMock;
        private Mock<ILogger<UserService>> _loggerMock;

        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _messagePublisherMock = new Mock<IMessagePublisher>();
            _loggerMock = new Mock<ILogger<UserService>>();

            _userService = new UserService(
                _userRepoMock.Object,
                _tokenServiceMock.Object,
                _messagePublisherMock.Object,
                _loggerMock.Object
            );
        }
        [Test]
        public void RegisterUser_ShouldReturnUserResponse_AndPublishEmail()
        {
            // Arrange 
            var registerDto = new RegisterUserDTO
            {
                FirstName = "Nishant",
                LastName = "Burnwal",
                Email = "nishant@test.com",
                Password = "Password@123"
            };

            var savedUser = new User
            {
                UserId = 1,
                FirstName = "Nishant",
                LastName = "Burnwal",
                Email = "nishant@test.com",
                Password = "hashed_password"
            };

            _userRepoMock
                .Setup(repo => repo.RegisterUser(It.IsAny<RegisterUserDTO>()))
                .Returns(savedUser);

            // call method
            var result = _userService.RegisterUser(registerDto);


            // checking data is not null
            Assert.That(result, Is.Not.Null);

            // Response data should match
            Assert.That(result.UserId, Is.EqualTo(savedUser.UserId));
            Assert.That(result.Email, Is.EqualTo(savedUser.Email));

            // Repository should be called once
            _userRepoMock.Verify(
                repo => repo.RegisterUser(It.IsAny<RegisterUserDTO>()),
                Times.Once
            );

            // Email register one time only
            _messagePublisherMock.Verify(
                publisher => publisher.PublishEmail(It.IsAny<EmailMessageDTO>()),
                Times.Once
            );
        }
        [Test]
        public void Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var loginDto = new LoginUserDTO
            {
                Email = "nishant@test.com",
                Password = "Password@123"
            };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Password@123");

            var user = new User
            {
                UserId = 1,
                Email = "nishant@test.com",
                Password = hashedPassword
            };

            _userRepoMock
                .Setup(repo => repo.GetUserByEmail(loginDto.Email))
                .Returns(user);

            _tokenServiceMock
                .Setup(token => token.GenerateToken(user))
                .Returns("fake-jwt-token");

            // Act
            var result = _userService.Login(loginDto);

            // 🔹 Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Email, Is.EqualTo(loginDto.Email));
            Assert.That(result.Token, Is.EqualTo("fake-jwt-token"));

            _tokenServiceMock.Verify(
                t => t.GenerateToken(user),
                Times.Once
            );
        }
        [Test]
        public void Login_ShouldThrowException_WhenPasswordIsInvalid()
        {
            // Arrange
            var loginDto = new LoginUserDTO
            {
                Email = "nishant@test.com",
                Password = "WrongPassword"
            };

            var user = new User
            {
                UserId = 1,
                Email = "nishant@test.com",
                Password = BCrypt.Net.BCrypt.HashPassword("CorrectPassword")
            };

            _userRepoMock
                .Setup(repo => repo.GetUserByEmail(loginDto.Email))
                .Returns(user);

            // Act & Assert
            Assert.Throws<InvalidCredentialsException>(() =>
                _userService.Login(loginDto)
            );
        }
        [Test]
        public void ForgotPassword_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            var dto = new ForgotPasswordDTO
            {
                Email = "test@gmail.com"
            };

            var user = new User
            {
                UserId = 1,
                Email = "test@gmail.com",
                FirstName = "Test"
            };

            _userRepoMock
                .Setup(repo => repo.GetUserByEmail(dto.Email))
                .Returns(user);

            //  Act
            var result = _userService.ForgotPassword(dto);

            // Assert
            Assert.That(result, Is.True);

            Assert.That(user.ResetToken, Is.Not.Null);
            Assert.That(user.ResetTokenExpiry, Is.Not.Null);

            // calling DB update
            _userRepoMock.Verify(
                repo => repo.UpdateUser(user),
                Times.Once
            );

            // RabbitMQ Email
            _messagePublisherMock.Verify(
                m => m.PublishEmail(It.IsAny<EmailMessageDTO>()),
                Times.Once
            );
        }
        [Test]
        public void ForgotPassword_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var dto = new ForgotPasswordDTO
            {
                Email = "notfound@gmail.com"
            };

            _userRepoMock
                .Setup(repo => repo.GetUserByEmail(dto.Email))
                .Returns((User)null);

            // Act
            var result = _userService.ForgotPassword(dto);

            // Assert
            Assert.That(result, Is.False);

            // Email not sent
            _messagePublisherMock.Verify(
                m => m.PublishEmail(It.IsAny<EmailMessageDTO>()),
                Times.Never
            );

            
            _userRepoMock.Verify(
                repo => repo.UpdateUser(It.IsAny<User>()),
                Times.Never
            );
        }
        // Reset Password Test Cases
        [Test]
        public void ResetPassword_ShouldReturnTrue_WhenTokenIsValid()
        {
            // 🔹 Arrange
            var dto = new ResetPasswordDTO
            {
                Token = "valid-token",
                NewPassword = "NewPassword@123"
            };

            var user = new User
            {
                UserId = 1,
                Email = "test@gmail.com",
                Password = "OLD_HASH",
                ResetToken = "valid-token",
                ResetTokenExpiry = DateTime.UtcNow.AddMinutes(10) // valid
            };

            _userRepoMock
                .Setup(repo => repo.GetUserByResetToken(dto.Token))
                .Returns(user);

            // Act
            var result = _userService.ResetPassword(dto);

            // Assert
            Assert.That(result, Is.True);

            Assert.That(
                BCrypt.Net.BCrypt.Verify(dto.NewPassword, user.Password),
                Is.True
            );

            // Token cleared
            Assert.That(user.ResetToken, Is.Null);
            Assert.That(user.ResetTokenExpiry, Is.Null);

            // DB update must happen
            _userRepoMock.Verify(
                repo => repo.UpdateUser(user),
                Times.Once
            );
        }
        //Test cases for Reset Password Failed(Suppose when Token is InValid)
        [Test]
        public void ResetPassword_ShouldReturnFalse_WhenTokenIsInvalid()
        {
            // Arrange
            var dto = new ResetPasswordDTO
            {
                Token = "invalid-token",
                NewPassword = "NewPassword@123"
            };

            _userRepoMock
                .Setup(repo => repo.GetUserByResetToken(dto.Token))
                .Returns((User)null);

            // Act
            var result = _userService.ResetPassword(dto);

            // Assert
            Assert.That(result, Is.False);

            // DB update should NOT happen
            _userRepoMock.Verify(
                repo => repo.UpdateUser(It.IsAny<User>()),
                Times.Never
            );
        }
        // Test cases for reset Password Failed(Suppose when Token Expired)
        [Test]
        public void ResetPassword_ShouldReturnFalse_WhenTokenIsExpired()
        {
            // 🔹 Arrange
            var dto = new ResetPasswordDTO
            {
                Token = "expired-token",
                NewPassword = "NewPassword@123"
            };

            var user = new User
            {
                ResetToken = "expired-token",
                ResetTokenExpiry = DateTime.UtcNow.AddMinutes(-5) // expired
            };

            _userRepoMock
                .Setup(repo => repo.GetUserByResetToken(dto.Token))
                .Returns(user);

            // Act
            var result = _userService.ResetPassword(dto);

            // Assert
            Assert.That(result, Is.False);

            // No DB update
            _userRepoMock.Verify(
                repo => repo.UpdateUser(It.IsAny<User>()),
                Times.Never
            );
        }
    }
}
