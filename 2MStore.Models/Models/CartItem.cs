namespace _2MStore.Models.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }


        public int CartId { get; set; }
        public Cart cart { get; set; }



        public virtual ICollection<Product> products { get; set;}
        //        CartItemId: Primary key.
        //CartId: Foreign key.
        //ProductId: Foreign key.
        //Quantity
    }
}
