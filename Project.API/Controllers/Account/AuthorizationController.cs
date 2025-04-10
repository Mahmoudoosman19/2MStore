



namespace Project.API.Controllers.Account
{
    [Authorize(Roles = "Admin")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthorizationController(IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager)
        {
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        [HttpGet(Router.AuthorizationRouting.GetAllRoles)]
        public async Task<ActionResult<List<GetAllRoles>>> GetAllRoles()
        {
            var Roles = await _authorizationService.GetRolesAsync();
            var mapRoles = Roles.Select(x => new GetAllRoles() { RoleName = x.Name, Id = x.Id }).ToList();

            return Ok(mapRoles);
        }
        [HttpPost(Router.AuthorizationRouting.AddRole)]
        public async Task<IActionResult> CreateRoleAsync([FromBody] AddRoleDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Role = await _authorizationService.AddRoleAsync(model.RoleName);
            if (Role == "RoleIsExist")
            { return BadRequest("Role Is Exist "); }
            else if (Role == "Created")
            {
                return Ok(new { mess = "Role Is Created" });
            }
            else
            {
                return BadRequest(new { mess = "Role Is Failure try Again" });
            }
        }

        [HttpPut(Router.AuthorizationRouting.UpdateRole)]
        public async Task<IActionResult> UpdateRoleAsync([FromBody] EditRoleDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var Role = await _authorizationService.UpdateRoleAsync(model.RoleName, model.Id);

            if (Role == "NotFound") { return NotFound(new { mess = "Role Is Not Found Try again" }); }
            else if (Role == "RoleExist") { return BadRequest(new { mess = "Role Is Exist Try again !" }); }
            else if (Role == "Failure")
            {
                return StatusCode(500, "An error occurred while updating the role. Please try again.");
            }
            else
            {
                return Ok(new { mess = "Role Is Update Successfull" });

            }
        }


        [HttpDelete(Router.AuthorizationRouting.DeleteRole)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var role = await _authorizationService.DeleteRoleAsync(id);
            if (role == "notfound") { return NotFound(new { mess = "Role Is Not Found !" }); }
            else if (role == "Delete") { return Ok(new { mess = "Delete Successfull" }); }
            else
            {
                return BadRequest();
            }
        }



        [HttpGet(Router.AuthorizationRouting.ManageUserRole)]
        public async Task<ActionResult<ManageUserRoles>> ManageUserRole(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound("User is not found");
            }

            var userManage = await _authorizationService.GetManageUserRoleWithRoles(user); // Await the async method

            if (userManage == null)
            {
                return BadRequest();
            }

            return Ok(userManage);
        }

        [HttpPost(Router.AuthorizationRouting.UpdateUserRole)]
        [AllowAnonymous]
        public async Task<ActionResult<UpdateUserRole>> UpdateUserRole([FromBody] UpdateUserRole userRole)
        {

            var userRoles = await _authorizationService.UpdateUserRoleWithRoles(userRole);
            if (userRoles == "notfound") { return NotFound(); }
            else if (userRoles == "FailedRemoveOldRoles") { return BadRequest(); }
            else if (userRoles == "FailedAddNewRoles") { return BadRequest(); }
            else if (userRoles == "Success") { return Ok(new { mess = "Updated SuccessFull" }); }
            else
            {
                return BadRequest();
            }




        }







    }
}
