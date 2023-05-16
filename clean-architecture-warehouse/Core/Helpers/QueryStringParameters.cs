namespace Core.Helpers;
public abstract class QueryStringParameters
{
    private const int _maxPageSize = 50;
    private int _pageSize = 2;
    public int CurrentPage { get; set; } = 1;

    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = value > _maxPageSize ? _maxPageSize : value;
        }
    }
}
