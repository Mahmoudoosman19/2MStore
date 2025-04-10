using Project.DAL.Entities.Identity;

namespace Project.BLL.Services
{
    public interface IApplicationUserServices
    {
        public Task<string> AddUser(ApplicationUser user, string Password,bool? flag);
    }
}
