using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apione.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecretController : ControllerBase
    {
        [Authorize]
        public IActionResult Index()
        {
            return Ok(new { message = "hello world" });
        }
    }
}