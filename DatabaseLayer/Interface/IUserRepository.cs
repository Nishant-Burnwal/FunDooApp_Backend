using ModelLayer.DTOs;
using ModelLayer.Entity;

namespace DatabaseLayer.Interface
{
    public interface IUserRepository
    {
        User RegisterUser(RegisterUserDTO registerUserDTO);
        User GetUserByEmail(string email);

        // Forgot Password
        bool SaveResetToken(string email, string token, DateTime expiry);
        User GetUserByResetToken(string token);
        bool UpdatePassword(User user, string newPassword);
        void UpdateUser(User user);

    }
}
