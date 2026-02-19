using SearchEngine.Domain.Documents;

namespace SearchEngine.Application.Abstractions.Persistence;

public interface IDocumentRepository
{
    Task<Document?> GetByIdAsync(DocumentId id, CancellationToken cancellationToken = default);

    Task AddAsync(Document document, CancellationToken cancellationToken = default);

    void Update(Document document);

    void Remove(Document document);
}
