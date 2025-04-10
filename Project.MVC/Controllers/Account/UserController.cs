using Newtonsoft.Json;
using Project.API.Helpers;
using Project.BLL.Dtos.Account;
using Project.BLL.Dtos.Authorization;
using Project.MVC.Services;
using System.Net.Http.Headers;
using System.Text;

namespace Project.MVC.Controllers.Account
{
    public class UserController : Controller
    {
        private readonly HttpClient _client;

        private readonly UserApiServices _apiService;
        private readonly RolesServices apiRoleService;

        public UserController()
        {

            _apiService = new UserApiServices();
            apiRoleService = new RolesServices();
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7276/Api/v1/Account/users/")
            };
        }


        public async Task<IActionResult> Index([FromQuery] UserSpecParams userSpecParams)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var query = $"https://localhost:7276/Api/v1/Account/users/List?PageIndex={userSpecParams.PageIndex}&PageSize=5";


            if (!string.IsNullOrEmpty(userSpecParams.Search))
            {
                query += $"&Search={userSpecParams.Search}";
            }

            var response = await _client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var paginatedUser = JsonConvert.DeserializeObject<Pagination<GetAllUsersDto>>(jsonResponse);

                int totalPages = (int)Math.Ceiling((double)paginatedUser.Count / userSpecParams.PageSize);

                ViewBag.PageIndex = paginatedUser.PageIndex;
                ViewBag.PageSize = paginatedUser.PageSize;
                ViewBag.TotalPages = totalPages;

                return View(paginatedUser.Data);
            }

            return View("Error");
        }

        public async Task<IActionResult> Create()
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
        public async Task<IActionResult> Create(RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var response = await _client.PostAsync("Register?flag=true", content);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();

                var errorDict = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(errorContent);

                if (errorDict != null)
                {
                    ViewBag.error = errorDict;
                    return View(model);

                }
                return View();
            }
            else
            {
                TempData["Update"] = "User is Created success";

                return RedirectToAction("Index");
            }



        }
        [HttpGet]
        public async Task<IActionResult> ManageRole(int id)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }
            var Roles = await apiRoleService.GetRolesAsync(accessToken);
            var userRoles = await apiRoleService.MangeRole(id, accessToken);
            ViewBag.Roles = Roles;

            return View(userRoles);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(int userId, List<int> selectedRoleIds)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }

            var userRoles = await apiRoleService.MangeRole(userId, accessToken);

            var model = new UpdateUserRole
            {
                UserId = userId,
                Roles = selectedRoleIds.Select(id => new Roles { RoleId = id, HasRole = true, RoleName = userRoles.Roles.Where(x => x.RoleId == id).Select(x => x.RoleName).FirstOrDefault() }).ToList()
            };

            var updateRoles = await apiRoleService.UpdateUserRole(model, accessToken);

            if (updateRoles != null)
            {
                TempData["Update"] = "Role is Updated success";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var accessToken = HttpContext.Session.GetString("AccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }
            var changePass = await _apiService.ChangePassword(model, accessToken);
            if (changePass)
            {
                TempData["Update"] = "Change Password Success";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CurrentPassword", "Password Is Invalid");
                return View(model);
            }

        }

    }
}