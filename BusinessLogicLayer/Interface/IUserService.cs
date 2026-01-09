using ModelLayer.DTOs;
using ModelLayer.DTOs.User;

namespace BusinessLogicLayer.Interface
{
    public interface IUserService
    {
        UserResponseDTO RegisterUser(RegisterUserDTO registerUserDTO);
        LoginResponseDTO Login(LoginUserDTO loginUserDTO);

        // Forgot Password
        bool ForgotPassword(ForgotPasswordDTO dto);
        bool ResetPassword(ResetPasswordDTO dto);
    }
}
