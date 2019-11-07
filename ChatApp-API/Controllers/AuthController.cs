using ChatApp.API.Data.Auth;
using ChatApp.API.JWT.Handlers;
using ChatApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatApp.API.Controllers
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
        public async Task<IActionResult> Register([FromBody]User user)
        {
            if (await authRepository.UserExist(user))
                return BadRequest("User already Exist.");

            var createdUser = await authRepository.Register(user);

            if (createdUser == null)
                return BadRequest("Something went wrong.");

            createdUser.Password = "";

            return Ok(createdUser);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]User user)
        {
            var userFromRepo = await authRepository.Login(user.Name, user.Password);

            if (userFromRepo == null)
                return Unauthorized();

            userFromRepo.Password = "";

            return Ok(jwtHandler.Create(userFromRepo));
        }

        [HttpGet]
        public IActionResult generateGuid()
        {

            var guidString = Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString();
            return Ok(guidString.Replace("-", ""));
        }
    }
}
