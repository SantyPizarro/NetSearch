using MediatR;
using static System.Net.Mime.MediaTypeNames;
public sealed record UpdateDocumentCommand(
    Guid Id,
    string Title,
    string Content,
    IEnumerable<string> Tags,
    string? Author,
    string? Category
) : IRequest<Unit>;