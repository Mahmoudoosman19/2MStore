namespace _2MStore.Models.Models
{
    public class Offers
    {
        public int OfferId { get; set; }
        public string Discription { get; set; }
        public float DiscountPrecentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int ProductId { get; set; }

        //        OfferId: Primary key.
        //Description
        //DiscountPercentage
        //ValidFrom
        //ValidTo
        //ProductId
    }
}
