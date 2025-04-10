using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Dtos.Product
{
    public class ProductBrandOrCategoryEditDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? PictureUrl { get; set; }
        public IFormFile? Url { get; set; }
    }
}
