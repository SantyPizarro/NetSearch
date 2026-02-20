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

        var termEntities = await _termRepository
            .GetByValuesAsync(query.Terms, cancellationToken);

        if (termEntities.Count == 0)
            return Array.Empty<SearchResult>();

        var allEntries = await _indexRepository
            .GetByTermIdsAsync(
                termEntities.Select(t => t.Id),
                cancellationToken);

        if (allEntries.Count == 0)
            return Array.Empty<SearchResult>();

        var groupedByDocument = allEntries
            .GroupBy(e => e.DocumentId)
            .ToList();

        var totalDocuments = await _documentRepository
            .CountAsync(cancellationToken);

        if (totalDocuments == 0)
            return Array.Empty<SearchResult>();

        var documentIds = groupedByDocument
            .Select(g => g.Key)
            .ToList();

        var documents = await _documentRepository
            .GetByIdsAsync(documentIds, cancellationToken);

        var documentDictionary = documents
            .ToDictionary(d => d.Id);

        var results = new List<SearchResult>();

        foreach (var group in groupedByDocument)
        {
            if (query.Operator == OperatorType.And)
            {
                var containsAllTerms = termEntities
                    .All(t => group.Any(e => e.TermId == t.Id));

                if (!containsAllTerms)
                    continue;
            }

            if (!documentDictionary.TryGetValue(group.Key, out var document))
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