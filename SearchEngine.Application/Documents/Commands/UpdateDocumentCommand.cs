using MediatR;

namespace SearchEngine.Application.Documents.Commands;

public sealed record UpdateDocumentCommand(
    Guid Id,
    string Title,
    string Content,
    IEnumerable<string> Tags,
    string? Author,
    string? Category
) : IRequest<Unit>;
