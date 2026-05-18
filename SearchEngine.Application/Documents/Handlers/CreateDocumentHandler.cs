using MediatR;
using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Application.Abstractions.Services;
using SearchEngine.Application.Documents.Commands;
using SearchEngine.Domain.Documents;

namespace SearchEngine.Application.Documents.Handlers;

public sealed class CreateDocumentHandler
    : IRequestHandler<CreateDocumentCommand, Guid>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IIndexingService _indexingService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateDocumentHandler(
        IDocumentRepository documentRepository,
        IIndexingService indexingService,
        IUnitOfWork unitOfWork)
    {
        _documentRepository = documentRepository;
        _indexingService = indexingService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateDocumentCommand request,
        CancellationToken cancellationToken)
    {
        var document = Document.Create(
            request.Title,
            request.Content,
            request.Tags,
            request.Author,
            request.Category);

        await _documentRepository.AddAsync(document, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _indexingService.IndexAsync(document, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return document.Id.Value;
    }
}
