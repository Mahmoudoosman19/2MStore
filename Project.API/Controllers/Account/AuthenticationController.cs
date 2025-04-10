


namespace Project.API.Controllers.Account
{

    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthenticationServices _authServices;
        public AuthenticationController(UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager,
            IAuthenticationServices authServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authServices = authServices;

        }

        [HttpPost(Router.AuthenticationRouting.SignIn)]
        public async Task<ActionResult<JWTAuthResult>> SignInAsync([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ModelState);
            }

            //1-if check user is Exist or not 
            var user = new EmailAddressAttribute().IsValid(model.EmailOrUserName) ?
              await _userManager.FindByEmailAsync(model.EmailOrUserName) :
              await _userManager.FindByNameAsync(model.EmailOrUserName);
            if (user == null) { return NotFound(new { mess = "Invalid Email Or Password" }); }
            //2-Check Password
            var found = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!found)
            {
                return NotFound(new { mess = "Invalid Email Or Password" });
            }
            //remember me
            await _signInManager.PasswordSignInAsync(user, model.Password, model.IsRememberMe, false);
            //confirm Email

            if (!user.EmailConfirmed)
            {
                return BadRequest(new { mess = "Email not Confirmed Try again !" });
            }


            //3-Generate Token
            var accesstoken = await _authServices.GetJWTToken(user);

            return Ok(accesstoken);


        }


        [HttpGet(Router.AuthenticationRouting.ConfirmEmail)]
        public async Task<ActionResult> ConfirmEmail(int? UserId, string? Code)
        {
            var Confirm = await _authServices.ConfirmEmail(UserId, Code);
            if (Confirm == "ErrorWhenConfirmEmail")
                return BadRequest(new { mess = "There Occur in Confirm Email Try again !" });
            else
            {
                return Redirect("http://localhost:4200/auth/login");
            }
        }
        [HttpPost(Router.AuthenticationRouting.SendCodeResetPassword)]
        public async Task<ActionResult<ResetPassword>> ResetPassword([FromBody]ResetPassword resetPassword)
        {

            var result = await _authServices.SendResetPasswordCode(resetPassword.Email);
            switch (result)
            {
                case "notfound": return NotFound(new { mess = "user not found" });
                case "FailedUpdate": return BadRequest(new { mess = "failed try again" });
                case "FailedResetPassword": return BadRequest(new { mess = "failed try again" });
                case "SendEmailSucccess": return Ok(new { mess = "Operation Is Done" });
                default:
                    return BadRequest();


            }
        }

        [HttpPost(Router.AuthenticationRouting.ConfirmResetPassword)]
        public async Task<ActionResult<ResetPasswordConfirm>> ConfirmResetPassword([FromBody]ResetPasswordConfirm Confirm)
        {


            var res = await _authServices.ConfirmResetPassword(Confirm.Code, Confirm.Email);
            switch (res)
            {
                case "notfound": return NotFound(new { mess = "user not found" });
                case "failed": return BadRequest(new { mess = "failed try again" });
                case "success": return Ok(new { mess = "Operation Is Done" });
                default:
                    return BadRequest(new { mess = "invalid Code Try again" });


            }

        }

        [HttpPost(Router.AuthenticationRouting.ResetPassword)]
        public async Task<ActionResult<ResetPass>> ResetPass([FromBody] ResetPass reset)
        {

            var result = await _authServices.ResetPassword(reset.Email, reset.Password);
            switch (result)
            {
                case "UserNotFound": return BadRequest(new { mess = "User not Found" });
                case "Failed": return BadRequest(new { mess = "Invalid Code Try again" });
                case "Success": return Ok(new { mess = "Change Password Success" });
                default: return BadRequest(new { mess = "Invalid Code Try again " });
            }
        }





    }
}
