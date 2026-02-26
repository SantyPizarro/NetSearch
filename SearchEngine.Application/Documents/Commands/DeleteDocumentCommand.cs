using MediatR;
using static System.Net.Mime.MediaTypeNames;

public sealed record DeleteDocumentCommand(
    Guid Id
) : IRequest<Unit>;