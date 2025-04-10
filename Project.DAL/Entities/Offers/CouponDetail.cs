using Project.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Entities.Offers
{
    public class CouponDetail : IBaseEntity
    {
        public int Id { get; set; }

        public string CouponCode { get; set; }

        public int Discount { get; set; }

        public double MaxDiscount { get; set; }

        public DateTime TilDate { get; set; }

        public List<ApplicationUser> applicationUsers { get; set; }

    }
}
