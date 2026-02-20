using Microsoft.EntityFrameworkCore;
using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Domain.Documents;
using SearchEngine.Domain.Indexing;

namespace SearchEngine.Infrastructure.Persistence.Repositories;

internal sealed class IndexRepository : IIndexRepository
{
    private readonly SearchEngineDbContext _context;

    public IndexRepository(SearchEngineDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        IndexEntry entry,
        CancellationToken cancellationToken = default)
    {
        await _context.IndexEntries.AddAsync(entry, cancellationToken);
    }

    public async Task<List<IndexEntry>> GetByTermIdsAsync(
           IEnumerable<TermId> termIds,
           CancellationToken cancellationToken = default)
    {
        var ids = termIds.Select(x => x.Value).ToList();

        return await _context.IndexEntries
            .Where(e => ids.Contains(e.TermId.Value))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<IndexEntry>> GetByDocumentIdAsync(
        DocumentId documentId,
        CancellationToken cancellationToken = default)
    {
        return await _context.IndexEntries
            .Where(e => e.DocumentId == documentId)
            .ToListAsync(cancellationToken);
    }

    public void RemoveRange(IEnumerable<IndexEntry> entries)
    {
        _context.IndexEntries.RemoveRange(entries);
    }

    public async Task DeleteByDocumentIdAsync(
    DocumentId documentId,
    CancellationToken cancellationToken = default)
    {
        var entries = await _context.IndexEntries
            .Where(e => e.DocumentId == documentId)
            .ToListAsync(cancellationToken);

        _context.IndexEntries.RemoveRange(entries);
    }

    public async Task DeleteByTermIdAsync(
        TermId termId,
        CancellationToken cancellationToken = default)
    {
        var entries = await _context.IndexEntries
            .Where(e => e.TermId == termId)
            .ToListAsync(cancellationToken);

        _context.IndexEntries.RemoveRange(entries);
    }

}
