namespace Talabat.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }

        //[ForeignKey(nameof(Product.Brand))]
        public int BrandId { get; set; } // Foregin key column => ProductBrand
        public ProductBrand Brand { get; set; } // Navigation Property [One]

        //[ForeignKey(nameof(Product.Category))]
        public int CategoryId { get; set; } // Foregin key column => ProductBrand
        public ProductCategory Category { get; set; } // Navigation Property [One]
    }
}
