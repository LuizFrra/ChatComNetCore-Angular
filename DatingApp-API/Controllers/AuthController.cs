using DatingApp.API.Data.Auth;
using DatingApp.API.JWT.Handlers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthRepository<User> authRepository;

        private IJwtHandler jwtHandler;

        public AuthController(IAuthRepository<User> authRepository, IJwtHandler jwtHandler)
        {
            this.authRepository = authRepository;
            this.jwtHandler = jwtHandler;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Register([FromBody]User user)
        {
            if (await authRepository.UserExist(user))
                return BadRequest("User already Exist.");

            var createdUser = await authRepository.Register(user);

            if (createdUser == null)
                return BadRequest("Something went wrong.");

            createdUser.PasswordHash = "";

            return Ok(createdUser);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Login([FromBody]User user)
        {
            var userFromRepo = await authRepository.Login(user.Name, user.PasswordHash);

            if (userFromRepo == null)
                return Unauthorized();

            userFromRepo.PasswordHash = "";



            return Ok(jwtHandler.Create(userFromRepo));
        }

        [HttpGet]
        [Authorize]
        public ActionResult<string> generateGuid()
        {

            var guidString = Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            return guidString.Replace("-", "");
        }
    }
}
