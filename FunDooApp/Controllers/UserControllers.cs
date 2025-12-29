using BusinessLogicLayer.Interface;
using DatabaseLayer.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;
using ModelLayer.Utility;

namespace FunDoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public IActionResult RegisterUser(RegisterUserDTO registerUserDTO)
        {
            var user = _userService.RegisterUser(registerUserDTO);

            return Ok(new ApiResponse<UserResponseDTO>
            {
                Success = true,
                Message = "Register Successful",
                Data = user
            });
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUserDTO loginUserDTO)
        {
            try
            {
                var result = _userService.Login(loginUserDTO);

                return Ok(new ApiResponse<LoginResponseDTO>
                {
                    Success = true,
                    Message = "Login Successful",
                    Data = result
                });
            }
            catch (InvalidCredentialsException ex)
            {
                return Unauthorized(new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [Authorize]
        [HttpGet("secure")]
        public IActionResult SecureApi()
        {
            return Ok("You are authenticated!");
        }
    }
}
