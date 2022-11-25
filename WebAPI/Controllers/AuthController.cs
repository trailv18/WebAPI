using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Core.Interfaces;
using WebAPI.Core.Models.User;

namespace WebAPI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel input)
        {
            var result = await _userService.Login(input);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel input)
        {
            var result = await _userService.Register(input);
            return Ok(result);
        }
    }
}
