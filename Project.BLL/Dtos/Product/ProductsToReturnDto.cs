using System.ComponentModel.DataAnnotations;

namespace Project.BLL.Dtos.Product
{
    public class ProductsToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        [Display(Name = "Original Price")]

        public decimal PriceBeforeDiscount { get; set; }

        public decimal Discount { get; set; }
        [Display(Name = "Net Price")]

        public decimal PriceAfterDiscount
        {
            get
            {
                return PriceBeforeDiscount - PriceBeforeDiscount * (Discount / 100);
            }
        }
        public int Count { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;


        public List<ImageToReturnDto> Images { get; set; } = new List<ImageToReturnDto>();

        public string PictureUrl { get; set; }

        public int BrandId { get; set; }
        public string Brand { get; set; }

        public int CategoryId { get; set; }
        public string Category { get; set; }
    }
}
