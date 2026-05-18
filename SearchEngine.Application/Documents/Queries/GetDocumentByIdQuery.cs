using MediatR;
using SearchEngine.Domain.Documents;

namespace SearchEngine.Application.Documents.Queries;

public sealed record GetDocumentByIdQuery(Guid Id)
    : IRequest<Document?>;
