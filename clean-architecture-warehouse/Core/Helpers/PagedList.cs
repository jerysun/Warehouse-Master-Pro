using Microsoft.EntityFrameworkCore;

namespace Core.Helpers;

public class PagedList<T> : List<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PagedList(List<T> items, int totalCount, int currentPage, int pageSize)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        AddRange(items);
    }

    public static async Task<PagedList<T>> ToPagedListAsync(IQueryable<T> source, int currentPage, int pageSize)
    {
        var totalCount = source.Count();
        var items = await source.Skip(currentPage * pageSize).Take(pageSize).ToListAsync();

        return new PagedList<T>(items, totalCount, currentPage, pageSize);
    }
}
