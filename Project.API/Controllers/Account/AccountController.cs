using System.Security.Claims;

namespace Project.API.Controllers.Account
{

    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmailServices _emailServices;
        private readonly IGenericRepository<ApplicationUser> _UserRepo;
        private readonly IApplicationUserServices _applicationUserServices;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper, IHttpContextAccessor contextAccessor,
            IEmailServices emailServices, IApplicationUserServices applicationUserServices, IGenericRepository<ApplicationUser> userRepo)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _emailServices = emailServices;
            _applicationUserServices = applicationUserServices;
            _UserRepo = userRepo;
        }

        [HttpGet(Router.AccountRouting.List)]
        public async Task<ActionResult<Pagination<GetAllUsersDto>>> GetAllUsers(UserSpecParams userSpec)
        {
            var spec = new UserSpecification(userSpec);

            var users = await _UserRepo.GetAllWithSpecAsync(spec);
            var Data = _mapper.Map<IReadOnlyList<GetAllUsersDto>>(users);



            var countSpec = new UserWithFiltersForCountSpecification(userSpec);

            var Count = await _UserRepo.GetCountAsync(countSpec);
            return Ok(new Pagination<GetAllUsersDto>(userSpec.PageIndex, userSpec.PageSize, Count, Data));


        }

        [HttpGet(Router.AccountRouting.GetById)]
        public async Task<IActionResult> GetUserByIdAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound(new { mess = $"ID={id} is not founded ", info = "Try Again" });
            return Ok(user);
        }

        [HttpPost(Router.AccountRouting.Register)]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto model, bool? flag = false)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.Phone,
                City = model.City,
                Country = model.Country,
                Street = model.Street,


            };



            var Result = await _applicationUserServices.AddUser(user, model.Password, flag);
            switch (Result)
            {
                case "EmailIsExist":
                    ModelState.AddModelError("Email", "Email Is Already Exist ! ");
                    return BadRequest(ModelState);
                case "UserNameIsExist":
                    ModelState.AddModelError("UserName", "UserName Is Already Exist ! ");
                    return BadRequest(ModelState);
                case "UserPhoneIsExist":
                    ModelState.AddModelError("Phone", "Phone Is Already Exist ! ");
                    return BadRequest(ModelState);
                case "FailedToTryRegister":
                    return BadRequest(new { mess = "Try to Register again" });
                case "MVCSucccess":
                    return Ok(new { mess = "add Employee Successfull" });
                case "SendEmailSucccess": return Ok(new { mess = "add Customer Successfull" });
                default:
                    return BadRequest(Result);

            }


        }
        [Authorize]
        [HttpPost(Router.AccountRouting.changePassword)]
        public async Task<IActionResult> changePassword([FromBody] ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null) { return NotFound(new { message = $"ID ={UserId} user don't Exist", info = "Try again!" }); }

            IdentityResult result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok("ChangePassword Is SuccessFull ");
            }

            return BadRequest("ChangePassword Is Failure");


        }


        [HttpPut(Router.AccountRouting.Edit)]
        public async Task<IActionResult> UpdateUserAsync(int Id, [FromBody] UpdateUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var OldUser = await _userManager.FindByIdAsync(Id.ToString());
            if (OldUser == null) { return NotFound("User Is Not Exist"); }

            var IsSameUserName = await _userManager.FindByNameAsync(model.UserName);
            if (IsSameUserName != null && IsSameUserName.Id != Id) { return BadRequest("UserName Is Exist "); }
            OldUser.FirstName = model.FirstName;
            OldUser.LastName = model.LastName;
            OldUser.UserName = model.UserName;
            OldUser.City = model.City;
            OldUser.Country = model.Country;
            OldUser.PhoneNumber = model.Phone;
            OldUser.Street = model.Street;

            var res = await _userManager.UpdateAsync(OldUser);
            if (res.Succeeded)
                return Ok("User Is Updated SuccessFull");


            return BadRequest("User Is Updated Failure");


        }




        [HttpDelete(Router.AccountRouting.Delete)]
        public async Task<IActionResult> DeleteUserAsync(int? id)
        {
            if (id == null)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            { return NotFound(new { message = $"ID ={id} user don't Exist", info = "Try again!" }); }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return Ok(new { mess = "Deleted Is Successfull" });

            return BadRequest(new { mess = "Deleted Is Failure" });
        }



    }
}
