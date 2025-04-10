using Microsoft.AspNetCore.Identity;
using Project.DAL.Entities.Offers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.DAL.Entities.Identity
{
    public class ApplicationUser : IdentityUser<int>, IBaseEntity
    {
        public ApplicationUser()
        {
            couponDetails = new HashSet<CouponDetail>();
            UserRefreshTokens = new HashSet<UserRefreshToken>();
        }
        public ICollection<CouponDetail> couponDetails { get; set; }

        [InverseProperty(nameof(UserRefreshToken.User))]
        public ICollection<UserRefreshToken> UserRefreshTokens { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Code { get; set; }
        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

    }
}
