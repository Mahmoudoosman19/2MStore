using Project.DAL.Entities;

namespace Project.BLL.Specifications
{
    public class ProductWithCategoryAndBrandSpecification : BaseSpecification<Product>
    {
        // this Constructor is used for get all products
        public ProductWithCategoryAndBrandSpecification(ProductSpecParams productSpecParams)
            : base(p =>
            (string.IsNullOrEmpty(productSpecParams.Search) || p.Name.ToLower().Contains(productSpecParams.Search)) &&
            (!productSpecParams.CategoryId.HasValue || p.CategoryId == productSpecParams.CategoryId.Value) &&  // Filteration
            (!productSpecParams.BrandId.HasValue || p.BrandId == productSpecParams.BrandId.Value)
            )
        {
            AddInclude(p => p.Brand);
            AddInclude(p => p.Category);
            AddInclude(p => p.Images);
            AddorderBy(p => p.Name);

            ApplyPagination(productSpecParams.PageSize * (productSpecParams.PageIndex - 1), productSpecParams.PageSize);

            if (!string.IsNullOrEmpty(productSpecParams.Sort))
            {
                switch (productSpecParams.Sort)
                {
                    case "priceAsc":
                        AddorderBy(p => p.PriceBeforeDiscount);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.PriceBeforeDiscount);
                        break;
                    default:
                        AddorderBy(p => p.Name);
                        break;
                }
            }
        }




        // this Constructor is used for get a specifiv product With Id

        public ProductWithCategoryAndBrandSpecification(int? id) : base(p => p.Id == id)
        {
            AddInclude(p => p.Brand);
            AddInclude(p => p.Category);
            AddInclude(p => p.Images);
            AddorderBy(p => p.Name);

        }


    }
}
