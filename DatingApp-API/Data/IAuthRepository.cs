using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public interface IAuthRepository<T> where T : User
    {
        Task<T> Register(T user);

        Task<T> Login(string userName, string password);

        Task<bool> UserExist(T user);
    }
}
