public interface ITermRepository
{
    Task<Term?> GetByValueAsync(string value, CancellationToken cancellationToken = default);

    Task<Term?> GetByIdAsync(TermId id, CancellationToken cancellationToken = default);

    Task AddAsync(Term term, CancellationToken cancellationToken = default);

    Task UpdateAsync(Term term, CancellationToken cancellationToken = default);
}
