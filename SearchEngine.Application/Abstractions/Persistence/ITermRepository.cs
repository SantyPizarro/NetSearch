using SearchEngine.Domain.Indexing;

public interface ITermRepository
{
    Task<List<Term>> GetByValuesAsync(
        IEnumerable<string> values,
        CancellationToken cancellationToken = default);

    Task<Term?> GetByIdAsync(TermId id, CancellationToken cancellationToken = default);

    Task AddAsync(Term term, CancellationToken cancellationToken = default);

    Task UpdateAsync(Term term, CancellationToken cancellationToken = default);

}
