using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2MStore.Models.Models.Identity
{
    public class User : IdentityUser<int>
    {
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }    
       
    }
}
