using Project.BLL.Dtos.Authorization;
using Project.DAL.Entities.Identity;

namespace Project.BLL.Services
{
    public interface IAuthorizationService
    {
        public Task<string> AddRoleAsync(string RoleName);
        public Task<string> UpdateRoleAsync(string RoleName, int id);
        public Task<List<Role>> GetRolesAsync();
        public Task<string> DeleteRoleAsync(int id);
        public Task<ManageUserRoles> GetManageUserRoleWithRoles(ApplicationUser user);
        public Task<string> UpdateUserRoleWithRoles(UpdateUserRole userRole);

    }
}
