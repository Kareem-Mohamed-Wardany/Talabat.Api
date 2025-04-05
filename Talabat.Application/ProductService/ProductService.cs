using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.Product_Specs;

namespace Talabat.Application.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<int> GetCountAsync(ProductSpecParams specParams)
        {
            var countSpec = new ProductsWithFilterationForCountSpecification(specParams);
            return _unitOfWork.Repository<Product>().GetCountAsync(countSpec);
        }

        public Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(specParams);

            return _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
        }

        public Task<Product?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);

            return _unitOfWork.Repository<Product>().GetWithSpecAsync(spec);
        }

        public Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        => _unitOfWork.Repository<ProductBrand>().GetAllAsync();


        public Task<IReadOnlyList<ProductCategory>> GetProductCategoriesAsync()
        => _unitOfWork.Repository<ProductCategory>().GetAllAsync();

    }
}
