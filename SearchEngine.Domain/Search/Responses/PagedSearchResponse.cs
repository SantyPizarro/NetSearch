using SearchEngine.Api.Contracts.Responses;

public sealed class PagedSearchResponse
{
    public int Total { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public IReadOnlyCollection<SearchResponse> Results { get; init; }
}