using Microsoft.AspNetCore.Mvc;
using PosNews.Models;
using PosNews.Services.Interfaces;

namespace PosNews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
           _authService = authService;
        }

        [HttpPost("Cadastrar")]
        public async Task<IActionResult> RegisterUser(RegisterUser user) 
        {
            var result = await _authService.RegisterUser(user);
            if(result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUser user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result =  await _authService.Login(user);

            if(result == true) 
            { 
                var tokenString = await _authService.GenerateTokenString(user);
                return Ok(tokenString);
            }

            return BadRequest();
        }

    }
}
