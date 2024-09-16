namespace _2MStore.Models.Models
{
    public class Category
    {
        public Category()
        {
            products = new List<Product>();


        }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }

        public virtual ICollection<Product> products { get; set; }
    }
}
