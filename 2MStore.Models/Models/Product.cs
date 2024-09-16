using System.ComponentModel.DataAnnotations.Schema;

namespace _2MStore.Models.Models
{
    public class Product
    {
        public Product()
        {
            ProductImgs = new List<ProductImgs>();
            OrderItems = new List<OrderItem>();

        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal Discount { get; set; }
        public decimal PriceAfterDiscount { get; set; }

        [ForeignKey("category")]
        public int CategoryId { get; set; }
        public Category category { get; set; }


        [ForeignKey("ProductDetails")]
        public int ProductDetailsID { get; set; }
        public ProductDetails ProductDetails { get; set; }

        public virtual ICollection<ProductImgs> ProductImgs { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }







    }
}
