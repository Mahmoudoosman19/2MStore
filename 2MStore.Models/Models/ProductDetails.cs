using System.ComponentModel.DataAnnotations.Schema;

namespace _2MStore.Models.Models
{
    public class ProductDetails
    {
        public int PDetailID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string? OtherFeatures { get; set; }

        [ForeignKey("product")]
        public int ProductId { get; set; }


        public Product product { get; set; }
    }
}
