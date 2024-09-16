namespace _2MStore.Models.Models
{
    public class Order
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int OrderStatus { get; set; }
        public string ShippingAddress { get; set; }
        public int ItemsCount { get; set; }
        public decimal TotalPrice { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }





    }
}
