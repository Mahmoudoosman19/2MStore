using Project.DAL.Entities;

namespace Project.BLL.Specifications
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecParams productSpecParams)
            : base(p =>
            (string.IsNullOrEmpty(productSpecParams.Search) || p.Name.ToLower().Contains(productSpecParams.Search)) && // Search
            (!productSpecParams.CategoryId.HasValue || p.CategoryId == productSpecParams.CategoryId.Value) &&  // Filteration
            (!productSpecParams.BrandId.HasValue || p.BrandId == productSpecParams.BrandId.Value)
            )
        {

        }
    }
}
