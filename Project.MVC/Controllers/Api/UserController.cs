using Microsoft.AspNetCore.Identity;
using Project.DAL.Entities.Identity;

namespace Project.MVC.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                    return Ok();
                }
                else
                {
                    return BadRequest(new { mess = "this user is not exist !" });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
    }



}

