public class RouteQueryParams
{
    private const int MaxPageSize = 10;
    private const int DefaultPageSize = 5;

    public int? DriverId { get; set; }
    public RouteSortingOptions SortingOption { get; set; }
    public string? SearchValue { get; set; }
    public int Page { get; set; } = 1;
    private int _pageSize { get; set; } = DefaultPageSize;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}

