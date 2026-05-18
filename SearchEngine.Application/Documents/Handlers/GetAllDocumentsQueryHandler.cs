using MediatR;
using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Application.Documents.Queries;
using SearchEngine.Domain.Documents;

namespace SearchEngine.Application.Documents.Handlers;

internal sealed class GetAllDocumentsQueryHandler
    : IRequestHandler<GetAllDocumentsQuery, List<Document>>
{
    private readonly IDocumentRepository _repository;

    public GetAllDocumentsQueryHandler(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Document>> Handle(
        GetAllDocumentsQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}
