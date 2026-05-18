using MediatR;

namespace SearchEngine.Application.Documents.Commands;

public sealed record DeleteDocumentCommand(
    Guid Id
) : IRequest<Unit>;
