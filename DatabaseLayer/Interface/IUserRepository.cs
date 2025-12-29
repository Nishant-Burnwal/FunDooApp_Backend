using ModelLayer.DTOs;
using ModelLayer.Entity;

namespace DatabaseLayer.Interface
{
    public interface IUserRepository
    {
        User RegisterUser(RegisterUserDTO registerUserDTO);
        User GetUserByEmail(string email);
    }
}
