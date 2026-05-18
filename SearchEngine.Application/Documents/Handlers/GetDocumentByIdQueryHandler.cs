using MediatR;
using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Application.Documents.Queries;
using SearchEngine.Domain.Documents;

namespace SearchEngine.Application.Documents.Handlers;

internal sealed class GetDocumentByIdQueryHandler
    : IRequestHandler<GetDocumentByIdQuery, Document?>
{
    private readonly IDocumentRepository _repository;

    public GetDocumentByIdQueryHandler(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Document?> Handle(
        GetDocumentByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(
            DocumentId.From(request.Id),
            cancellationToken);
    }
}
