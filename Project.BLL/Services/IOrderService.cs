using Project.BLL.Specifications.OrderSpecs;
using Project.DAL.Entities.Order;

namespace Project.BLL.Services
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address addressShiping);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
        Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
        Task<IReadOnlyList<Order?>> GetAllOrdersAsync(OrderSpecParams orderSpecParams);
        Task<IReadOnlyList<Order>> GetOrderCount(OrderSpecParams orderSpecParams);
    }
}
