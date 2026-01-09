using BCrypt.Net;
using BusinessLogicLayer.Interface;
using DatabaseLayer.Exceptions;
using DatabaseLayer.Interface;
using Microsoft.Extensions.Logging;
using ModelLayer.DTOs;
using ModelLayer.DTOs.Email;
using ModelLayer.DTOs.User;
using ModelLayer.Entity;
using System.Security.Cryptography;

namespace BusinessLogicLayer.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IMessagePublisher messagePublisher,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        // Register User
        public UserResponseDTO RegisterUser(RegisterUserDTO registerUserDTO)
        {
            _logger.LogInformation("Registering user with email {Email}", registerUserDTO.Email);

            registerUserDTO.Password =
                BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password);

            var user = _userRepository.RegisterUser(registerUserDTO);
            // Log Before publishing email
            _logger.LogInformation(
                "Sending welcome email to {Email}",
            user.Email
            );

            // Send welcome email via RabbitMQ
            _messagePublisher.PublishEmail(new EmailMessageDTO
            {
                To = user.Email,
                Subject = "Welcome to FunDoo Notes",
                Body = $"Hi {user.FirstName},<br/><br/>Welcome to FunDoo Notes!"
            });

            _logger.LogInformation("User registered successfully with email {Email}", user.Email);

            return new UserResponseDTO
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }

        // Login User
        public LoginResponseDTO Login(LoginUserDTO loginUserDTO)
        {
            _logger.LogInformation("Login attempt for email {Email}", loginUserDTO.Email);

            var user = _userRepository.GetUserByEmail(loginUserDTO.Email);

            if (user == null ||
                !BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.Password))
            {
                _logger.LogWarning("Invalid login attempt for email {Email}", loginUserDTO.Email);
                throw new InvalidCredentialsException("Invalid Email or Password");
            }

            var token = _tokenService.GenerateToken(user);

            _logger.LogInformation("Login successful for email {Email}", loginUserDTO.Email);

            return new LoginResponseDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                Token = token
            };
        }

        // Forgot Password
        public bool ForgotPassword(ForgotPasswordDTO dto)
        {
            _logger.LogInformation("Forgot password requested for email {Email}", dto.Email);

            var user = _userRepository.GetUserByEmail(dto.Email);
            if (user == null)
            {
                _logger.LogWarning("Forgot password requested for non-existing email {Email}", dto.Email);
                return false;
            }

            user.ResetToken = GenerateResetToken();
            user.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);

            _userRepository.UpdateUser(user);
_messagePublisher
            .PublishEmail(new EmailMessageDTO
            {
                To = user.Email,
                Subject = "Reset Your FunDoo Password",
                Body = $@"
                    Hi {user.FirstName},<br/><br/>
                    <b>Reset Token:</b> {user.ResetToken}<br/><br/>
                    This token expires in 15 minutes.
                "
            });

            _logger.LogInformation("Reset password email queued for {Email}", user.Email);

            return true;
        }

        // Reset Password
        public bool ResetPassword(ResetPasswordDTO dto)
        {
            _logger.LogInformation("Reset password attempt using token");

            var user = _userRepository.GetUserByResetToken(dto.Token);

            if (user == null || user.ResetTokenExpiry < DateTime.UtcNow)
            {
                _logger.LogWarning("Invalid or expired reset token");
                return false;
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            _userRepository.UpdateUser(user);

            _logger.LogInformation("Password reset successful for user {Email}", user.Email);

            return true;
        }

        // Helper
        private string GenerateResetToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
