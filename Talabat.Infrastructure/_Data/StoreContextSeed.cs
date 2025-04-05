using System.Text.Json;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Infrastructure.Data
{
    public static class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext _dbContext)
        {
            string path = "../Talabat.Infrastructure/_Data/DataSeed";

            if (!_dbContext.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText($"{path}/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands?.Count() > 0)
                {
                    foreach (var item in brands)
                    {
                        _dbContext.Set<ProductBrand>().Add(item);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

            if (!_dbContext.ProductCategories.Any())
            {
                var categoriesData = File.ReadAllText($"{path}/categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);

                if (categories?.Count() > 0)
                {
                    foreach (var item in categories)
                    {
                        _dbContext.Set<ProductCategory>().Add(item);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

            if (!_dbContext.Products.Any())
            {
                Console.WriteLine("Entered");

                var productsData = await File.ReadAllTextAsync($"{path}/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                Console.WriteLine(products);

                if (products?.Count() > 0)
                {
                    foreach (var item in products)
                    {
                        _dbContext.Set<Product>().Add(item);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

            if (!_dbContext.DeliveryMethods.Any())
            {
                var DeliveryMethodData = File.ReadAllText($"{path}/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);

                if (deliveryMethods?.Count() > 0)
                {
                    foreach (var item in deliveryMethods)
                    {
                        _dbContext.Set<DeliveryMethod>().Add(item);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

        }
    }
}
