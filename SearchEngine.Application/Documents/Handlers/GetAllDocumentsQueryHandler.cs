using MediatR;
using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Domain.Documents;

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