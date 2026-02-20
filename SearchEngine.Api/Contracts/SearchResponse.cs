namespace SearchEngine.Api.Contracts.Responses;

public sealed record SearchResponse(
    Guid DocumentId,
    string Title,
    double Score);