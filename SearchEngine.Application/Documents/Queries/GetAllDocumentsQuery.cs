using MediatR;
using SearchEngine.Domain.Documents;

namespace SearchEngine.Application.Documents.Queries;

public sealed record GetAllDocumentsQuery()
    : IRequest<List<Document>>;
