using Microsoft.EntityFrameworkCore;
using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Domain.Indexing;

namespace SearchEngine.Infrastructure.Persistence.Repositories;

internal sealed class TermRepository : ITermRepository
{
    private readonly SearchEngineDbContext _context;

    public TermRepository(SearchEngineDbContext context)
    {
        _context = context;
    }

    public async Task<List<Term>> GetByValuesAsync(
        IEnumerable<string> values,
        CancellationToken cancellationToken = default)
    {
        var normalized = values
            .Select(v => v.Trim().ToLowerInvariant())
            .ToList();

        return await _context.Terms
            .Where(t => normalized.Contains(t.Value))
            .ToListAsync(cancellationToken);
    }

    public async Task<Term?> GetByIdAsync(
        TermId id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Terms
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task AddAsync(
        Term term,
        CancellationToken cancellationToken = default)
    {
        await _context.Terms.AddAsync(term, cancellationToken);
    }

    public void Update(Term term)
    {
        _context.Terms.Update(term);
    }

    public Task UpdateAsync(Term term, CancellationToken cancellationToken = default)
    {
        _context.Terms.Update(term);
        return Task.CompletedTask;
    }

}
