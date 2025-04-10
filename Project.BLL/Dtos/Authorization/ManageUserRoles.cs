using System.ComponentModel.DataAnnotations;

namespace Project.BLL.Dtos.Authorization
{

    public class ManageUserRoles
    {
        public int UserId { get; set; }
        [Display(Name = "User Name")]
        public string username { get; set; }
        public List<Roles> Roles { get; set; }
        public List<int> SelectedRoleIds { get; set; } // Or List<string> if RoleId is string

    }

    public class Roles
    {
        public int RoleId { get; set; }
        [Display(Name = "Role Name")]

        public string RoleName { get; set; }

        public bool HasRole { get; set; }
    }
}