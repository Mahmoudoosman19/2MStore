using Microsoft.AspNetCore.Http;

namespace Project.BLL.Dtos.Product
{
    public class ProductBrandOrCategoryToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public IFormFile Url { get; set; }
    }
}
