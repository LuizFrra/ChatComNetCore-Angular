using DatingApp.API.Models;
using DatingApp.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCrypt.Net;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository<User>
    {

        private DataContext dataContext;

        public AuthRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<User> Login(string userName, string password)
        {
            var user = await dataContext.Users.FirstOrDefaultAsync(u => u.Name == userName);

            if (user == null)
                return await Task.FromResult<User>(null);

            if(!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return await Task.FromResult<User>(null);

            //await ReHashPassword(user, password);
            user.PasswordHash = "";

            return await Task.FromResult(user);
        }

        private async Task ReHashPassword(User user, string password)
        {
            if(BCrypt.Net.BCrypt.PasswordNeedsRehash(user.PasswordHash, 11))
            {
                var newPasswordHash = CreatePasswordHash(password);

                user.PasswordHash = newPasswordHash;

                dataContext.Update(user);

                await dataContext.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }

        public async Task<User> Register(User user)
        {
            if (await UserExist(user))
                return null;
            
            user.PasswordHash = CreatePasswordHash(user.PasswordHash);

            await dataContext.AddAsync<User>(user);

            int addUserResult = await dataContext.SaveChangesAsync();

            user.PasswordHash = "";

            if (addUserResult == 1)
                return await Task.FromResult(user);

            return await Task.FromResult<User>(null);
        }

        private string CreatePasswordHash(string passwordHash)
        {
            return BCrypt.Net.BCrypt.HashPassword(passwordHash, 11);
        }

        public async Task<bool> UserExist(User user)
        {
            var userResult = await dataContext.Users.FirstOrDefaultAsync(u => u.Name == user.Name);

            if (userResult == null)
                return await Task.FromResult(false);

            return await Task.FromResult(true);
        }
    }
}
