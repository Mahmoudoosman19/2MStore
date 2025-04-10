using Project.DAL.Entities.Order;

namespace Project.BLL.Specifications.OrderSpecs
{
    public class OrderSpecificationCount : BaseSpecification<Order>
    {
        public OrderSpecificationCount(OrderSpecParams orderSpecParams) : base(o =>
           (string.IsNullOrEmpty(orderSpecParams.Search) || o.BuyerEmail.ToLower().Contains(orderSpecParams.Search)) &&
           (!orderSpecParams.Status.HasValue || o.Status == orderSpecParams.Status.Value) &&
           (!orderSpecParams.Tracking.HasValue || o.OrderTrackingStatus == orderSpecParams.Tracking.Value)
           )
        {

        }
    }
}
