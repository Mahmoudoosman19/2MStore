namespace _2MStore.Models.Models
{
    public class CartItem
    {

        public CartItem()
        {
            products = new List<Product>();
            Carts = new List<Cart>();
        }
        public int CartItemId { get; set; }
        public int Quantity { get; set; }


        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Product> products { get; set; }

    }
}
