using Microsoft.EntityFrameworkCore;

namespace Shared.Dal.Pagination;

public class PaginationFilter
{
    private int _pageNumber;
    private int _pageSize;

    public PaginationFilter(
        int pageNumber,
        int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 10 ? value : 10;
    }
}

public class PaginationResponse<TEntity>
{
    public PaginationResponse()
    {
        Items = new List<TEntity>();
        Succeeded = true;
    }

    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public IReadOnlyCollection<TEntity>? Items { get; set; }
    public bool Succeeded { get; set; }
}

public static class PaginationExtensions
{
    public static async Task<PaginationResponse<TEntity>> PaginateAsync<TEntity>(
        this IQueryable<TEntity> query,
        PaginationFilter filter,
        CancellationToken cancellationToken = default)
    {
        var response = new PaginationResponse<TEntity>();

        response.CurrentPage = filter.PageNumber;
        response.PageSize = filter.PageSize;
        response.TotalItems = await query.CountAsync(cancellationToken);

        response.Items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        response.TotalPages = (int)Math.Ceiling(response.TotalItems / (double)filter.PageSize);

        return response;
    }
}