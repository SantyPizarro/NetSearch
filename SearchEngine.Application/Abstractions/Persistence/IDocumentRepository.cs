using SearchEngine.Domain.Documents;

namespace SearchEngine.Application.Abstractions.Persistence;

public interface IDocumentRepository
{
    Task<Document?> GetByIdAsync(DocumentId id, CancellationToken ct);
    Task<int> GetTotalCountAsync(CancellationToken ct);
    Task AddAsync(Document document, CancellationToken ct);
    Task UpdateAsync(Document document, CancellationToken ct);
    Task DeleteAsync(Document document, CancellationToken ct);
}
