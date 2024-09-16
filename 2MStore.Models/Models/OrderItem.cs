namespace _2MStore.Models.Models
{
    public class OrderItem
    {

        public OrderItem()
        {
            Orders = new List<Order>();
            Products = new List<Product>();

        }
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }


        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Product> Products { get; set; }


        //        OrderItemId: Primary key.
        //OrderId: Foreign key.
        //ProductId: Foreign key.
        //Quantity
        //PriceAtPurchase
    }
}
