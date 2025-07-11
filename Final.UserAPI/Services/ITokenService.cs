using Final.Domain.Entities;

namespace Final.UserAPI.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
