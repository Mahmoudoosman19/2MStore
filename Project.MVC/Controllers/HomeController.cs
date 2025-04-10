using Newtonsoft.Json;
using Project.API.Helpers;
using Project.BLL.Dtos.Account;
using Project.BLL.Dtos.Product;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;


namespace Project.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        HttpClient _client = new HttpClient();



        public async Task<IActionResult> Index(ProductSpecParams productSpecParams)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var query = $"https://localhost:7276/Api/v1/Products/Pagination?PageIndex={productSpecParams.PageIndex}&PageSize=5";
            var response = await _client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                // Deserialize into PaginatedResponse<ProductDto>
                var paginatedProducts = JsonConvert.DeserializeObject<Pagination<ProductsToReturnDto>>(jsonResponse);



                //********************Users********************************//




                var query2 = $"https://localhost:7276/Api/v1/Account/users/List?PageSize=5";

                var res = await _client.GetAsync(query2);

                if (res.IsSuccessStatusCode)
                {
                    var jsonRes = await res.Content.ReadAsStringAsync();
                    var paginatedUser = JsonConvert.DeserializeObject<Pagination<GetAllUsersDto>>(jsonRes);
                    ViewBag.Users = paginatedUser;
                }
                else
                {
                    return View();
                }












                return View(paginatedProducts);
            }
            else
            {
                return View();

            }

        }
        public List<string> GetUserRolesFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var roleClaims = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role)
                                            .Select(c => c.Value).ToList();
            return roleClaims;
        }
        public IActionResult GetUserRoles()
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");

            var roleClaims = GetUserRolesFromToken(accessToken);
            ViewBag.Roles = roleClaims;
            return View();
        }



    }
}
