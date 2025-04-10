using Microsoft.AspNetCore.Http;

namespace Project.BLL.Dtos.Product
{
    public class CategoryOrBrandCreateDto
    {
        public string Name { get; set; }
        public IFormFile Url { get; set; }

    }
}
