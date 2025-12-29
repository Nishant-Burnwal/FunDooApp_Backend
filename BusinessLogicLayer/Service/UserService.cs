using BusinessLogicLayer.Interface;
using DatabaseLayer.Interface;
using DatabaseLayer.Exceptions;
using ModelLayer.DTOs;
using ModelLayer.Entity;
using BCrypt.Net;


namespace BusinessLogicLayer.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserResponseDTO RegisterUser(RegisterUserDTO registerUserDTO)
        {
            registerUserDTO.Password = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password);

            var user = _userRepository.RegisterUser(registerUserDTO);

            return new UserResponseDTO
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }


        public LoginResponseDTO Login(LoginUserDTO loginUserDTO)
        {
            var user = _userRepository.GetUserByEmail(loginUserDTO.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.Password))
            {
                throw new InvalidCredentialsException("Invalid Email or Password");
            }

            var token = _tokenService.GenerateToken(user);

            return new LoginResponseDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                Token = token
            };
        }
    }
}
