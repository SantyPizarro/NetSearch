namespace SearchEngine.Api.Contracts.Responses;

public sealed class SearchResponse
{
    public Guid DocumentId { get; init; }
    public string Title { get; init; } = default!;
    public double Score { get; init; }
}