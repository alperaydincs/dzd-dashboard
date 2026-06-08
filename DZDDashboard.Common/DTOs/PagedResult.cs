namespace DZDDashboard.Common.DTOs;

/// <summary>Generic paged response envelope.</summary>
public sealed record PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}

/// <summary>Common pagination query parameters.</summary>
public sealed record PaginationParams
{
    public const int DefaultPageSize = 50;
    public const int MaxPageSize     = 200;

    public int Page     { get; init; } = 1;
    public int PageSize { get; init; } = DefaultPageSize;

    public int NormalizedPage     => Math.Max(1, Page);
    public int NormalizedPageSize => Math.Clamp(PageSize, 1, MaxPageSize);
}
