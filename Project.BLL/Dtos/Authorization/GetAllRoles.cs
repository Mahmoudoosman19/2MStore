using System.ComponentModel.DataAnnotations;

namespace Project.BLL.Dtos.Authorization
{
    public class GetAllRoles
    {
        [Display(Name = "Role Name")]

        public string RoleName { get; set; }
        public int Id { get; set; }

    }
}
