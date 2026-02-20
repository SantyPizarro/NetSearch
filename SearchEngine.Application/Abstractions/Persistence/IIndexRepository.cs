using SearchEngine.Domain.Documents;
using SearchEngine.Domain.Indexing;

public interface IIndexRepository
{
    Task AddAsync(IndexEntry entry, CancellationToken cancellationToken = default);

    Task<List<IndexEntry>> GetByTermIdsAsync(
        IEnumerable<TermId> termIds,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<IndexEntry>> GetByDocumentIdAsync(
        DocumentId documentId,
        CancellationToken cancellationToken = default);

    Task DeleteByDocumentIdAsync(
        DocumentId documentId,
        CancellationToken cancellationToken = default);

    Task DeleteByTermIdAsync(
        TermId termId,
        CancellationToken cancellationToken = default);


}
