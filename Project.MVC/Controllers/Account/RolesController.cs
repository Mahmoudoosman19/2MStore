using Newtonsoft.Json;
using Project.BLL.Dtos.Authorization;
using Project.MVC.Services;
using Project.MVC.ViewModel;
using System.Net.Http.Headers;
using System.Text;

namespace Project.MVC.Controllers.Account
{
    public class RolesController : Controller
    {
        private readonly HttpClient _client;

        private readonly RolesServices _apiService;

        public RolesController()
        {
            _apiService = new RolesServices();
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7276/Api/v1/Authorization/")
            };
        }
        public async Task<ActionResult<IReadOnlyList<GetAllRoles>>> Index()
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }

            var Roles = await _apiService.GetRolesAsync(accessToken);
            return View(Roles);
        }

        public async Task<IActionResult> AddRole()
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");


            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleDto model)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("AddRole", content);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();


                if (errorContent != null)
                {
                    ViewBag.error = errorContent;
                    return View(model);

                }
                return View();
            }
            else
            {
                TempData["Create"] = "Role is Created success";

                return RedirectToAction(nameof(Index));
            }


        }

        public async Task<IActionResult> EditRole(int id)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            if (id == 0)
            {
                return BadRequest();
            }
            var Roles = await _apiService.GetRolesAsync(accessToken);
            var role = Roles.Select(x => x).Where(x => x.Id == id).FirstOrDefault();
            var model = new EditRoleDto()
            {
                Id = id,
                RoleName = role.RoleName
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleDto model)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("UpdateRole", content);

            if (!response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResponseError>(jsonResponse);

                if (result != null)
                {
                    ViewBag.error = result.mess;
                    return View(model);

                }
                return View();
            }
            else
            {
                TempData["Update"] = "Role is Created success";

                return RedirectToAction(nameof(Index));
            }


        }

    }
}
