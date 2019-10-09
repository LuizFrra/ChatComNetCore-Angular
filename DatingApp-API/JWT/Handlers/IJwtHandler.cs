using DatingApp.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.JWT.Handlers
{
    public interface IJwtHandler
    {
        TokenValidationParameters Parameters { get; }

        Jwt Create(User user);
    }
}
