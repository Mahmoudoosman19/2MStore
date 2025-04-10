using Project.BLL.Dtos;
using Project.DAL.Entities.Order;

namespace Project.API.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;
        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
                return $"{_configuration["ApiUrl"]}{source.ItemOrdered.PictureUrl}";
            return null;

        }
    }
}
