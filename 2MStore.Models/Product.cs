using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2MStore.Models
{
    internal class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("PDetailID")]
        public int ProductDetails {  get; set; }


    }
}
