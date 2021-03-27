using System.Threading.Tasks;
using auth.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signinManager;

        public TestController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signinManager = signInManager;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { result = "Hello world" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("secret")]
        public IActionResult Secret()
        {
            return Ok(new { result = "Hello secret" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Request request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                return NotFound("Username not found");
            }

            var siginResult = await _signinManager.PasswordSignInAsync(user, request.Password, false, false);

            return Ok(new { message = "", user = user });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Request request)
        {
            var user = new IdentityUser() { UserName = request.Username };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "Admin");

            return Ok("Hello world");
        }
    }

    public class Request
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}