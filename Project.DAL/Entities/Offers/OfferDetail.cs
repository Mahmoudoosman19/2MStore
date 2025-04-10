using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Entities.Offers
{
    public class OfferDetail:IBaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Discount  { get; set; }

        public DateTime TIlDate { get; set; }

        public List<Product> products { get; set; } = new List<Product>();
    }
}
