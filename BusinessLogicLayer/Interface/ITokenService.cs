using ModelLayer.Entity;

namespace BusinessLogicLayer.Interface
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
