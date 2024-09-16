using System.ComponentModel.DataAnnotations.Schema;

namespace _2MStore.Models.Models
{
    public class ProductImgs
    {
        public int ImgId { get; set; }

        public string ImgPath { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

    }
}
