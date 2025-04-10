using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Entities
{
    public class Category:IBaseEntity, IHasPictureUrl
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PictureUrl { get; set; }

    }
}
