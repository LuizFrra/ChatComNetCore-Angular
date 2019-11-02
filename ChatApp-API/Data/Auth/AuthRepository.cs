using ChatApp.API.Models;
using ChatApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ChatApp.API.Data.Auth
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

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                return await Task.FromResult<User>(null);

            //await ReHashPassword(user, password);
            user.Password = "";
            
            if(string.IsNullOrEmpty(user.ImagePath))
                user.ImagePath = "";

            return await Task.FromResult(user);
        }

        private async Task ReHashPassword(User user, string password)
        {
            if (BCrypt.Net.BCrypt.PasswordNeedsRehash(user.Password, 11))
            {
                var newPasswordHash = CreatePasswordHash(password);

                user.Password = newPasswordHash;

                dataContext.Update(user);

                await dataContext.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }

        public async Task<User> Register(User user)
        {
            if (await UserExist(user))
                return null;

            user.Password = CreatePasswordHash(user.Password);

            await dataContext.AddAsync<User>(user);

            int addUserResult = await dataContext.SaveChangesAsync();

            user.Password = "";

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
