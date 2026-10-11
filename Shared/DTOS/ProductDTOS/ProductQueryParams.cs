
namespace Shared.DTOS.ProductDTOS
{
    public class ProductQueryParams
    {
        public int? TypeId { get; set; }

        public int? BrandId { get; set; }

        public string? Search { get; set; }

        public Sort Sort { get; set; }

        private int _pageIndex = 1;

        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value <= 0 ? 1 : value;
        }

        private const int DefaultSize = 5;
        private const int MaxSize = 10;

        private int _pageSize = DefaultSize;

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value <= 0)
                    _pageSize = DefaultSize;
                else if (value > MaxSize)
                    _pageSize = MaxSize;
                else
                    _pageSize = value;
            }
        }
    }
}
