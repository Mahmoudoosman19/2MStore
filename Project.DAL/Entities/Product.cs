namespace Project.DAL.Entities
{
    public class Product : BaseEntity, IHasPictureUrl, IHasListImages
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public bool ISOffer { get; set; }

        public String Description { get; set; }

        public decimal PriceBeforeDiscount { get; set; }

        private decimal discount;
        private const decimal maxdiscount = 70;

        public decimal Discount
        {

            get
            { return discount; }

            set
            { discount = (value < maxdiscount) ? value : maxdiscount; }

        }

        public decimal PriceAfterDiscount
        {
            get
            {
                return Math.Round(PriceBeforeDiscount - (PriceBeforeDiscount * (Discount / 100)), 2);
            }
        }
        public int Count { get; set; } 

        public int Point { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        public string PictureUrl { get; set; }

        public List<Image> Images { get; set; }

        public List<Detail> Details { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }


    }
}
