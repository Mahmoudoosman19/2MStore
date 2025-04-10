using Project.DAL.Entities.Order;

namespace Project.BLL.Specifications.OrderSpecs
{
    public class OrderSpecification : BaseSpecification<Order>
    {

        public OrderSpecification(OrderSpecParams orderSpecParams) : base(o =>
           (string.IsNullOrEmpty(orderSpecParams.Search) || o.BuyerEmail.ToLower().Contains(orderSpecParams.Search)) &&
           (!orderSpecParams.Status.HasValue || o.Status == orderSpecParams.Status.Value) &&
           (!orderSpecParams.Tracking.HasValue || o.OrderTrackingStatus == orderSpecParams.Tracking.Value)
           )
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

            AddOrderByDescending(o => o.OrderDate);
            ApplyPagination(orderSpecParams.PageSize * (orderSpecParams.PageIndex - 1), orderSpecParams.PageSize);

        }
        public OrderSpecification()
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

            AddOrderByDescending(o => o.OrderDate);
        }

        public OrderSpecification(int orderId) : base(o => o.Id == orderId)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

        }
        public OrderSpecification(string buyerEmail) : base(o => o.BuyerEmail == buyerEmail)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

            AddOrderByDescending(o => o.OrderDate);
        }

        public OrderSpecification(int orderId, string buyerEmail) : base(o => o.Id == orderId && o.BuyerEmail == buyerEmail)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);


        }


    }
}
