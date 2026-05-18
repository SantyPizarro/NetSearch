namespace SearchEngine.Domain.Search.Responses;

public sealed class SearchResponse
{
    public Guid DocumentId { get; init; }
    public string Title { get; init; } = default!;
    public double Score { get; init; }
    public string Snippet { get; init; } = string.Empty;
}
