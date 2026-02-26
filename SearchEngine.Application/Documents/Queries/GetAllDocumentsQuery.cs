using MediatR;
using SearchEngine.Domain.Documents;

public sealed record GetAllDocumentsQuery()
    : IRequest<List<Document>>;