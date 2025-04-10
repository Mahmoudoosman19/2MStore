using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Data;
using Project.DAL.Entities.Identity;

namespace Project.BLL.Services
{
    public class ApplicationUserServices : IApplicationUserServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailServices _emailServices;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly IUrlHelper Url;

        public ApplicationUserServices(UserManager<ApplicationUser> userManager
            , IEmailServices emailServices
            , IHttpContextAccessor contextAccessor,
            ApplicationDbContext context,
             IUrlHelper Url)
        {
            _userManager = userManager;
            _emailServices = emailServices;
            _contextAccessor = contextAccessor;
            _context = context;
            this.Url = Url;

        }

        public async Task<string> AddUser(ApplicationUser user, string password, bool? flag = false)
        {
            var trans = _context.Database.BeginTransaction();

            try
            {
                //the best be  in  flunent validation
                var SameEmail = await _userManager.FindByEmailAsync(user.Email);
                if (SameEmail != null)
                {
                    return "EmailIsExist";
                }
                var SameUserName = await _userManager.FindByNameAsync(user.UserName);
                if (SameUserName != null)
                {

                    return "UserNameIsExist";
                }
                var SameUserPhone = await _context.Users.Where(x => x.PhoneNumber == user.PhoneNumber).FirstOrDefaultAsync();
                if (SameUserPhone != null)
                {

                    return "UserPhoneIsExist";
                }


                // send confirm email

                if (flag == false)
                {

                    IdentityResult Result = await _userManager.CreateAsync(user, password);
                    if (!Result.Succeeded)
                        return string.Join(",", Result.Errors.Select(x => x.Description).ToList());
                    await _userManager.AddToRoleAsync(user, "user");
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var requestaccessor = _contextAccessor.HttpContext.Request;
                    var returnUrl = $"{requestaccessor.Scheme}://{requestaccessor.Host}" + Url.Action("ConfirmEmail", "Authentication", new
                    {
                        UserId = user.Id,
                        Code = code,
                        ischeck = false
                    });
                    //$"/Api/Authentication/ConfirmEmail?UserId={user.Id}&Code={code}";
                    var message = $"Welcome <b>{user.UserName}<b> To Confirm Email: <a href='{returnUrl}'>Click Here </a>";
                    await _emailServices.SendEmailAsync(user.Email, message, "Confirm Email");
                    await trans.CommitAsync();
                    return "SendEmailSucccess";

                }
                else
                {

                    user.EmailConfirmed = true;
                    IdentityResult result = await _userManager.CreateAsync(user, password);

                    if (!result.Succeeded)
                    {
                        var errorMessages = string.Join(",", result.Errors.Select(x => x.Description).ToList());
                        Console.WriteLine("User creation failed: " + errorMessages);
                        return errorMessages;
                    }
                    await trans.CommitAsync();

                    return "MVCSucccess";
                }




            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return "FailedToTryRegister";
            }
        }
    }

}