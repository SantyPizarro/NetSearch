namespace SearchEngine.Api.Contracts.Requests;

public sealed record UpdateDocumentRequest(
    string Title,
    string Content,
    IEnumerable<string> Tags,
    string? Author,
    string? Category
);