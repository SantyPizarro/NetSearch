using MediatR;
using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Application.Abstractions.Services;
using SearchEngine.Application.Documents.Commands;
using SearchEngine.Domain.Documents;

namespace SearchEngine.Application.Documents.Handlers;

public sealed class UpdateDocumentHandler : IRequestHandler<UpdateDocumentCommand, Unit>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IIndexingService _indexingService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDocumentHandler(
        IDocumentRepository documentRepository,
        IIndexingService indexingService,
        IUnitOfWork unitOfWork)
    {
        _documentRepository = documentRepository;
        _indexingService = indexingService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        UpdateDocumentCommand request,
        CancellationToken cancellationToken)
    {
        var documentId = DocumentId.From(request.Id);

        var document = await _documentRepository
            .GetByIdAsync(documentId, cancellationToken);

        if (document is null)
            throw new KeyNotFoundException($"Document with id '{request.Id}' not found.");

        document.Update(
            request.Title,
            request.Content,
            request.Tags,
            request.Author,
            request.Category);

        _documentRepository.Update(document);

        await _indexingService.IndexAsync(document, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
