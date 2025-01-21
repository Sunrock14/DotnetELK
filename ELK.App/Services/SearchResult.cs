namespace ELK.App.Services;

public class SearchResult<T>
{
    public List<T> Items { get; set; }
    public long Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)Total / PageSize);
}
