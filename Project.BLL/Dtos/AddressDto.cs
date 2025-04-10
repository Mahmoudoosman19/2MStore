using System.ComponentModel.DataAnnotations;

namespace Project.BLL.Dtos
{
    public class AddressDto
    {

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Street { get; set; }
        public string? City { get; set; }

    }
}