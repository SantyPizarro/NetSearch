using SearchEngine.Domain.Indexing;

namespace SearchEngine.Application.Abstractions.Persistence;

public interface ITermRepository
{
    Task<Term?> GetByValueAsync(string value, CancellationToken ct);
    Task AddAsync(Term term, CancellationToken ct);
    Task UpdateAsync(Term term, CancellationToken ct);
}
