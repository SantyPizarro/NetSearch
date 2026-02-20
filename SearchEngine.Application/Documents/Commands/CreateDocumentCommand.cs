using MediatR;

public sealed record CreateDocumentCommand(
    string Title,
    string Content,
    IEnumerable<string> Tags,
    string? Author,
    string? Category
) : IRequest<Guid>;