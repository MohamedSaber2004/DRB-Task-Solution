public class PaginatedResult<TEntity>
{
    public PaginatedResult(int page, int pageSize, int totalCount, List<TEntity> items)
    {
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        Items = items;
    }

    public int Page { get; set; }
    public int PageSize { get; set; }
    public int ItemsCount => Items.Count;
    public int TotalCount { get; set; }
    public List<TEntity> Items { get; set; } = [];
}
