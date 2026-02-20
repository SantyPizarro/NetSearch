namespace SearchEngine.Api.Contracts.Responses;

public sealed record DocumentResponse(
    Guid Id,
    string Title,
    string Content);