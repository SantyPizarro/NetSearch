namespace SearchEngine.Api.Contracts.Requests;

public sealed record CreateDocumentRequest(
    string Title,
    string Content,
    IEnumerable<string> Tags,
    string? Author,
    string? Category);