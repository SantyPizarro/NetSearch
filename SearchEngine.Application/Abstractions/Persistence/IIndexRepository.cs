using SearchEngine.Domain.Documents;
using SearchEngine.Domain.Indexing;

namespace SearchEngine.Application.Abstractions.Persistence;

public interface IIndexRepository
{
    Task AddAsync(IndexEntry entry, CancellationToken ct);

    Task<IEnumerable<IndexEntry>> GetByDocumentIdAsync(
        DocumentId documentId,
        CancellationToken ct);

    Task DeleteByDocumentIdAsync(
        DocumentId documentId,
        CancellationToken ct);
}
