using MediatR;

public sealed record GetDocumentByIdQuery(Guid Id)
    : IRequest<Document?>;