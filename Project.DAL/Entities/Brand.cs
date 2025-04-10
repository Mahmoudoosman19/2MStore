using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Entities
{
    public class Brand:IBaseEntity , IHasPictureUrl
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PictureUrl { get; set; }

    }
}
