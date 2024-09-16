namespace _2MStore.Models.Models
{
    public class Cart
    {
        public Cart()
        {
            CartItems = new List<CartItem>();
        }
        public int CartId { get; set; }
        public int UserId { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }


    }
}
