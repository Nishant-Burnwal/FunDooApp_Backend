using BusinessLogicLayer.Interface;
using DatabaseLayer.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;
using ModelLayer.DTOs.User;
using ModelLayer.Utility;

namespace FunDoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterUserDTO dto)
        {
            _logger.LogInformation("Register API called for Email: {Email}", dto.Email);

            var user = _userService.RegisterUser(dto);

            _logger.LogInformation("User registered successfully: {Email}", dto.Email);

            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUserDTO dto)
        {
            _logger.LogInformation("Login attempt for Email: {Email}", dto.Email);

            try
            {
                var result = _userService.Login(dto);
                _logger.LogInformation("Login successful for Email: {Email}", dto.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Login failed for Email: {Email}", dto.Email);
                throw;
            }
        }

        [Authorize]
        [HttpGet("secure")]
        public IActionResult SecureApi()
        {
            return Ok("You are authenticated!");
        }

        // Forgot Password
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(ForgotPasswordDTO dto)
        {
            bool result = _userService.ForgotPassword(dto);

            if (!result)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Email not registered"
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Password reset link sent to email"
            });
        }
        // Reset Password
        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordDTO dto)
        {
            bool result = _userService.ResetPassword(dto);

            if (!result)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid or expired token"
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Password reset successful"
            });
        }
    }
}
