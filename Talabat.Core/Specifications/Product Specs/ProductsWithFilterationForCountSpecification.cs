using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProductsWithFilterationForCountSpecification : BaseSpecifications<Product>
    {
        public ProductsWithFilterationForCountSpecification(ProductSpecParams specParams)
            : base(p =>
                    (string.IsNullOrEmpty(specParams.search) || p.Name.ToLower().Contains(specParams.search.ToLower())) &&
                    (!specParams.brandId.HasValue || p.BrandId == specParams.brandId.Value) &&
                    (!specParams.categoryId.HasValue || p.CategoryId == specParams.categoryId.Value)
            )
        {

        }

    }
}
