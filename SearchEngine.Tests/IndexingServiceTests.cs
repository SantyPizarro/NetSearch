using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Application.Abstractions.Services;
using SearchEngine.Domain.Documents;
using SearchEngine.Domain.Indexing;
using SearchEngine.Infrastructure.Indexing;

namespace SearchEngine.Tests;

public sealed class IndexingServiceTests
{
    [Fact]
    public async Task IndexAsync_UpdatesDocumentFrequencyWhenDocumentIsReindexed()
    {
        var termRepository = new InMemoryTermRepository();
        var indexRepository = new InMemoryIndexRepository();
        var service = new IndexingService(
            termRepository,
            indexRepository,
            new SplitTokenizer(),
            new NoOpUnitOfWork());

        var document = Document.Create(
            "Doc",
            "alpha alpha beta",
            Array.Empty<string>(),
            null,
            null);

        await service.IndexAsync(document, CancellationToken.None);

        Assert.Equal(1, termRepository.Find("alpha")?.DocumentFrequency);
        Assert.Equal(1, termRepository.Find("beta")?.DocumentFrequency);
        Assert.Equal(2, indexRepository.Entries.Single(e => e.TermId == termRepository.Find("alpha")!.Id).TermFrequency);

        document.Update(
            "Doc",
            "beta gamma",
            Array.Empty<string>(),
            null,
            null);

        await service.IndexAsync(document, CancellationToken.None);

        Assert.Equal(0, termRepository.Find("alpha")?.DocumentFrequency);
        Assert.Equal(1, termRepository.Find("beta")?.DocumentFrequency);
        Assert.Equal(1, termRepository.Find("gamma")?.DocumentFrequency);
        Assert.DoesNotContain(indexRepository.Entries, e => e.TermId == termRepository.Find("alpha")!.Id);
        Assert.Contains(indexRepository.Entries, e => e.TermId == termRepository.Find("gamma")!.Id);
    }

    private sealed class InMemoryTermRepository : ITermRepository
    {
        private readonly List<Term> _terms = new();

        public Term? Find(string value)
        {
            return _terms.SingleOrDefault(t => t.Value == value);
        }

        public Task<List<Term>> GetByValuesAsync(
            IEnumerable<string> values,
            CancellationToken cancellationToken = default)
        {
            var normalized = values.Select(v => v.Trim().ToLowerInvariant()).ToHashSet();
            return Task.FromResult(_terms.Where(t => normalized.Contains(t.Value)).ToList());
        }

        public Task<Term?> GetByIdAsync(TermId id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_terms.SingleOrDefault(t => t.Id == id));
        }

        public Task AddAsync(Term term, CancellationToken cancellationToken = default)
        {
            _terms.Add(term);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Term term, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class InMemoryIndexRepository : IIndexRepository
    {
        private readonly List<IndexEntry> _entries = new();

        public IReadOnlyCollection<IndexEntry> Entries => _entries;

        public Task AddAsync(IndexEntry entry, CancellationToken cancellationToken = default)
        {
            _entries.Add(entry);
            return Task.CompletedTask;
        }

        public Task<List<IndexEntry>> GetByTermIdsAsync(
            IEnumerable<TermId> termIds,
            CancellationToken cancellationToken = default)
        {
            var ids = termIds.ToHashSet();
            return Task.FromResult(_entries.Where(e => ids.Contains(e.TermId)).ToList());
        }

        public Task<IReadOnlyList<IndexEntry>> GetByDocumentIdAsync(
            DocumentId documentId,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<IndexEntry>>(
                _entries.Where(e => e.DocumentId == documentId).ToList());
        }

        public Task DeleteByDocumentIdAsync(
            DocumentId documentId,
            CancellationToken cancellationToken = default)
        {
            _entries.RemoveAll(e => e.DocumentId == documentId);
            return Task.CompletedTask;
        }

        public Task DeleteByTermIdAsync(TermId termId, CancellationToken cancellationToken = default)
        {
            _entries.RemoveAll(e => e.TermId == termId);
            return Task.CompletedTask;
        }
    }

    private sealed class SplitTokenizer : ITokenizer
    {
        public IReadOnlyCollection<string> Tokenize(string text)
        {
            return text
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => x.ToLowerInvariant())
                .ToList();
        }
    }

    private sealed class NoOpUnitOfWork : IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }
    }
}
