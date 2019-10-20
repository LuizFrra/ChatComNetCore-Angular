using ChatApp.API.Models;
using System.Threading.Tasks;

namespace ChatApp.API.Data.Auth
{
    public interface IAuthRepository<T> where T : User
    {
        Task<T> Register(T user);

        Task<T> Login(string userName, string password);

        Task<bool> UserExist(T user);
    }
}
