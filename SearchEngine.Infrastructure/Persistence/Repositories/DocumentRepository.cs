using Microsoft.EntityFrameworkCore;
using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Domain.Documents;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SearchEngine.Infrastructure.Persistence.Repositories;

internal sealed class DocumentRepository : IDocumentRepository
{
    private readonly SearchEngineDbContext _context;

    public DocumentRepository(SearchEngineDbContext context)
    {
        _context = context;
    }

    public async Task<Document?> GetByIdAsync(
        DocumentId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Documents
            .Include(d => d.Tags)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }
    public async Task<List<Document>> GetByIdsAsync(
       IEnumerable<DocumentId> ids,
       CancellationToken cancellationToken = default)
    {
        var values = ids.Select(x => x.Value).ToList();

        return await _context.Documents
            .Where(d => values.Contains(d.Id.Value))
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        Document document,
        CancellationToken cancellationToken = default)
    {
        await _context.Documents.AddAsync(document, cancellationToken);
    }

    public void Update(Document document)
    {
        _context.Documents.Update(document);
    }

    public void Remove(Document document)
    {
        _context.Documents.Remove(document);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Documents.CountAsync(cancellationToken);
    }
}