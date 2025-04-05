namespace Talabat.Core.Specifications.Product_Specs
{
    public class ProductSpecParams
    {
        public string? sort { get; set; }
        public int? brandId { get; set; }
        public int? categoryId { get; set; }

        private const int MaxPageSize = 10;

        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 10 ? MaxPageSize : value; }
        }

        public int PageIndex { get; set; } = 1;
        public string? search { get; set; }

    }
}
