using MediatR;
using SearchEngine.Domain.Documents;

public sealed record GetDocumentByIdQuery(Guid Id)
    : IRequest<Document?>;