using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using client.Models;
using System.Net.Http;
using IdentityModel.Client;

namespace client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClient;

        public HomeController(
            ILogger<HomeController> logger,
            IHttpClientFactory httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var serverClient = _httpClient.CreateClient();
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("http://localhost:3001");
            var tokenRespone = await serverClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = "client_id",
                    ClientSecret = "client_secret",
                    Scope = "ApiOne",
                });

            var apiClient = _httpClient.CreateClient();

            apiClient.SetBearerToken(tokenRespone.AccessToken);

            var respone = await apiClient.GetAsync("http://localhost:3000/secret");

            var content = await respone.Content.ReadAsStringAsync();

            return Ok(new
            {
                access_token = tokenRespone.AccessToken,
                messgae = content,
            });
            // return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
