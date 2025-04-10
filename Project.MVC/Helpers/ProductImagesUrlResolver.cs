using AutoMapper;
using Project.BLL.Dtos.Product;

namespace Project.MVC.Helpers
{
    public class ProductImagesUrlResolver<T, TDto> : IValueResolver<T, TDto, List<ImageToReturnDto>> where T : IHasListImages

    {

        private readonly IConfiguration _configuration;

        public ProductImagesUrlResolver(IConfiguration configuration)

        {
            _configuration = configuration;
        }

        public List<ImageToReturnDto> Resolve(T source, TDto destination, List<ImageToReturnDto> destMember, ResolutionContext context)

        {

            return source.Images?.Select(img => new ImageToReturnDto

            {

                Id = img.Id,

                Name = $"{_configuration["ApiUrl"]}{img.PictureUrl}"

            }).ToList();

        }

    }

}