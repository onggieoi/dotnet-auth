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
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { result = "Hello world" });
        }

        // [Authorize(Roles = "Admin")]
        [HttpGet("secret")]
        public IActionResult Secret()
        {
            return Ok(new { result = "Hello secret" });
        }
    }

    public class Request
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}