using Microsoft.AspNetCore.Http;
using Project.BLL.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Project.BLL.Dtos.Product
{
    public class ProductDto
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
                return PriceBeforeDiscount - (PriceBeforeDiscount * (Discount / 100));
            }
        }
        public int Count { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public string? PictureUrl { get; set; }
        [AllowedExtensions(Helper.AllowExtensions)]
        [MaxFileSize(Helper.MaxFileSizInBytes)]
        public IFormFile Url { get; set; }
        [AllowedExtensions(Helper.AllowExtensions)]
        [MaxFileSize(Helper.MaxFileSizInBytes)]
        public List<IFormFile> ImagesUrl { get; set; }


        public int BrandId { get; set; }
        public string? Brand { get; set; }

        public int CategoryId { get; set; }
        public string? Category { get; set; }

        public List<ImageToReturnDto> Images { get; set; } = new List<ImageToReturnDto>();

        public List<DetailDto> Details { get; set; } = new List<DetailDto>();

    }
}