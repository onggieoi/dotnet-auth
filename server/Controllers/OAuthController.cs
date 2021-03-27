using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace auth.Controllers
{
    // [ApiController]
    [Route("oauth")]
    public class OAuthController : Controller
    {
        [HttpGet("Authorize")]
        public IActionResult Authorize(
            string respone_type,
            string client_id,
            string redirect_uri,
            string scope,
            string state
        )
        {
            var query = new QueryBuilder();
            query.Add("redirectUri", redirect_uri);
            query.Add("state", state);

            return View(model: query.ToString());
        }

        [HttpPost("authorize")]
        public IActionResult Authorize(
            string username,
            string redirectUri,
            string state)
        {
            const string code = "code";

            var query = new QueryBuilder();

            query.Add("code", code);
            query.Add("state", state);

            return Redirect($"{redirectUri}{query.ToString()}");
        }

        [HttpGet("token")]
        public async Task<IActionResult> Token(
            string grant_type,
            string code,
            string redirect_uri,
            string client_id)
        {
            // validate code
            var access_token = "access_token";

            var resObj = new
            {
                access_token,
                token_type = "Bearer",
                raw_claim = "Oauth"
            };

            var resJson = JsonConvert.SerializeObject(resObj);
            var resBytes = Encoding.UTF8.GetBytes(resJson);

            await Response.Body.WriteAsync(resBytes, 0, resBytes.Length);

            return Redirect(redirect_uri);
        }

        public IActionResult Index()
        {
            return Ok(new { message = "Hello World" });
        }
    }
}