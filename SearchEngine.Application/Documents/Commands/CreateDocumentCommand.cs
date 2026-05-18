using MediatR;

namespace SearchEngine.Application.Documents.Commands;

public sealed record CreateDocumentCommand(
    string Title,
    string Content,
    IEnumerable<string> Tags,
    string? Author,
    string? Category
) : IRequest<Guid>;
