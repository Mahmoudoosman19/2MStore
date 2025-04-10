using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Project.BLL.Dtos.Authentication;
using Project.DAL.Entities.Identity;
using Project.MVC.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project.MVC.Controllers.Account
{

    public class AccountController : Controller
    {
        private readonly HttpClient _client;
        private readonly SignInManager<ApplicationUser> _signInManager;
        Uri baseAddress = new Uri("https://localhost:7276/Api/v1/");

        public AccountController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;

        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateReCaptcha]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string data = JsonConvert.SerializeObject(model);
                    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "Authentication/SignIn", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();

                        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsonResponse);

                        var handler = new JwtSecurityTokenHandler();
                        var jwtSecurityToken = handler.ReadJwtToken(tokenResponse.AccessToken);

                        var roles = jwtSecurityToken.Claims
                            .Where(claim => claim.Type == ClaimTypes.Role)
                            .Select(claim => claim.Value)
                            .ToList();
                        var role = roles.Contains("Admin");
                        if (role)
                        {

                            HttpContext.Session.SetString("AccessToken", tokenResponse.AccessToken);
                            TempData["Create"] = $"Welcome, {tokenResponse.RefreshToken.UserName}";
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Not Authorized for Only Admin");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Password Or EmailOrUserName.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Remove("AccessToken");
            return RedirectToAction("Index", "Home");
        }
    }
}