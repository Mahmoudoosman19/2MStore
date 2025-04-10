
using AutoMapper;

namespace Project.API.Helpers
{
    public class PictureUrlResolver<T, TDto> : IValueResolver<T, TDto, string> where T : IHasPictureUrl
    {
        private readonly IConfiguration _configuration;

        public PictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(T source, TDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["ApiUrl"]}{source.PictureUrl}";
            return null;
        }
    }
}
