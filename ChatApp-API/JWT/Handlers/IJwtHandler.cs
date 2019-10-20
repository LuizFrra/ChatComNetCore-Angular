using ChatApp.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.API.JWT.Handlers
{
    public interface IJwtHandler
    {
        TokenValidationParameters Parameters { get; }

        Jwt Create(User user);
    }
}
