using ModelLayer.DTOs;

namespace BusinessLogicLayer.Interface
{
    public interface IUserService
    {
        UserResponseDTO RegisterUser(RegisterUserDTO registerUserDTO);
        LoginResponseDTO Login(LoginUserDTO loginUserDTO);
    }
}
