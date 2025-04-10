namespace Project.DAL.Entities.Order
{
    public class Order : BaseEntity
    {

        // Empty parameterless constructor
        public Order()
        {

        }
        public Order(string buyerEmail, Address shipToAddress, DeliveryMethod deliveryMethod, List<OrderItem> items, decimal subTotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;

            ShippingAddress = shipToAddress;
            Items = items;
            SubTotal = subTotal;

            DeliveryMethod = deliveryMethod;
            PaymentIntentId = paymentIntentId;
        }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public Address ShippingAddress { get; set; }
        public string BuyerEmail { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public OrderTracking OrderTrackingStatus { get; set; } = OrderTracking.InStock;

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public DeliveryMethod DeliveryMethod { get; set; }

        public decimal SubTotal { get; set; }

        public decimal GetTotal()
            => SubTotal + DeliveryMethod.Cost;

        public string? PaymentIntentId { get; set; }
    }
}
