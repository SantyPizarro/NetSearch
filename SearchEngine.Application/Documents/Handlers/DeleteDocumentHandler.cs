using MediatR;
using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Domain.Documents;
using static System.Net.Mime.MediaTypeNames;

namespace SearchEngine.Application.Documents.Handlers;

public sealed class DeleteDocumentHandler : IRequestHandler<DeleteDocumentCommand, Unit>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IIndexRepository _indexRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDocumentHandler(
        IDocumentRepository documentRepository,
        IIndexRepository indexRepository,
        IUnitOfWork unitOfWork)
    {
        _documentRepository = documentRepository;
        _indexRepository = indexRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(
        DeleteDocumentCommand request,
        CancellationToken cancellationToken)
    {
        var documentId = DocumentId.From(request.Id);

        var document = await _documentRepository
            .GetByIdAsync(documentId, cancellationToken);

        if (document is null)
            throw new KeyNotFoundException($"Document with id '{request.Id}' not found.");

        document.MarkAsDeleted();

        await _indexRepository.DeleteByDocumentIdAsync(document.Id, cancellationToken);

        _documentRepository.Remove(document);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}