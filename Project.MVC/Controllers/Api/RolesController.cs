using Project.MVC.Services;

namespace Project.MVC.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {



        private readonly RolesServices _apiService;

        public RolesController()
        {
            _apiService = new RolesServices();
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveRole(int id)
        {
            var accessToken = HttpContext.Session.GetString("AccessToken");

            if (string.IsNullOrEmpty(accessToken))
            {
                return RedirectToAction("Login", "Account");
            }

            var response = await _apiService.RemoveRole(id, accessToken);
            if (response == "Success")
            {
                return Ok();
            }
            else
            {
                return Ok(response);
            }


        }

    }
}
