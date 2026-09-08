namespace Shared.DTOS.ProductDTOS
{
  public  class ProductQueryParams
    {
    public int? TypeId { get; set; }
    public int? BrandId { get; set; }
    public string? Search { get; set; }
    public Sort Sort { get; set; }
    public int _pageIndex = 1;
    public int PageIndex
    {
        get
        {
            return _pageIndex;
        }
        set
        {
            _pageIndex = (value <= 0) ? 1 : value;
        }
    }
    private int DefultSize = 5;
    private int MaxSize = 10;

    public int _pageindex = 5;

    public int PagerIndex
    {
        get
        {
            return _pageIndex;
        }
        set
        {
            if (value <= 0)
                _pageIndex = DefultSize;
            else if (value > MaxSize)
                _pageIndex = MaxSize;
            else
                _pageIndex = value;
        }
    }



}
}