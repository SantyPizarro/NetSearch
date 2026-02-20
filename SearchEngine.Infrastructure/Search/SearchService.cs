using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Application.Abstractions.Services;
using SearchEngine.Domain.Indexing;
using SearchEngine.Domain.Search;

namespace SearchEngine.Infrastructure.Search;

public sealed class SearchService : ISearchService
{
    private readonly IIndexRepository _indexRepository;
    private readonly IDocumentRepository _documentRepository;
    private readonly ITermRepository _termRepository;
    private readonly IRankingStrategy _rankingStrategy;

    public SearchService(
        IIndexRepository indexRepository,
        IDocumentRepository documentRepository,
        ITermRepository termRepository,
        IRankingStrategy rankingStrategy)
    {
        _indexRepository = indexRepository;
        _documentRepository = documentRepository;
        _termRepository = termRepository;
        _rankingStrategy = rankingStrategy;
    }

    public async Task<IReadOnlyCollection<SearchResult>> SearchAsync(
        SearchQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.Terms.Count == 0)
            return Array.Empty<SearchResult>();

        // 1️⃣ Resolver entidades Term
        var termEntities = new List<Term>();

        foreach (var termValue in query.Terms)
        {
            var term = await _termRepository
                .GetByValueAsync(termValue, cancellationToken);

            if (term is not null)
                termEntities.Add(term);
        }

        if (termEntities.Count == 0)
            return Array.Empty<SearchResult>();

        // 2️⃣ Obtener entradas del índice
        var allEntries = new List<IndexEntry>();

        foreach (var term in termEntities)
        {
            var entries = await _indexRepository
                .GetByTermIdAsync(term.Id, cancellationToken);

            allEntries.AddRange(entries);
        }

        if (allEntries.Count == 0)
            return Array.Empty<SearchResult>();

        // 3️⃣ Agrupar por documento
        var groupedByDocument = allEntries
            .GroupBy(e => e.DocumentId);

        var totalDocuments = await _documentRepository
            .CountAsync(cancellationToken);

        if (totalDocuments == 0)
            return Array.Empty<SearchResult>();

        var results = new List<SearchResult>();

        foreach (var group in groupedByDocument)
        {
            // 4️⃣ Operador AND
            if (query.Operator == OperatorType.And)
            {
                var containsAllTerms = termEntities
                    .All(t => group.Any(e => e.TermId == t.Id));

                if (!containsAllTerms)
                    continue;
            }

            var document = await _documentRepository
                .GetByIdAsync(group.Key, cancellationToken);

            if (document is null)
                continue;

            double score = 0;

            foreach (var term in termEntities)
            {
                var entry = group
                    .FirstOrDefault(e => e.TermId == term.Id);

                if (entry is not null)
                {
                    score += _rankingStrategy
                        .CalculateScore(term, entry.TermFrequency, totalDocuments);
                }
            }

            results.Add(new SearchResult(document, score));
        }

        return results
            .OrderByDescending(x => x.Score)
            .ToList();
    }
}