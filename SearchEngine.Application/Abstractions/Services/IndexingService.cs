using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Application.Abstractions.Services;
using SearchEngine.Domain.Documents;
using SearchEngine.Domain.Indexing;

namespace SearchEngine.Application.Indexing.Services;

public sealed class IndexingService
{
    private readonly ITokenizer _tokenizer;
    private readonly ITermRepository _termRepository;
    private readonly IIndexRepository _indexRepository;

    public IndexingService(
        ITokenizer tokenizer,
        ITermRepository termRepository,
        IIndexRepository indexRepository)
    {
        _tokenizer = tokenizer;
        _termRepository = termRepository;
        _indexRepository = indexRepository;
    }

    public async Task IndexDocumentAsync(
        Document document,
        CancellationToken ct)
    {
        var fullText = $"{document.Title} {document.Content}";

        var tokens = _tokenizer.Tokenize(fullText);

        var termFrequencies = tokens
            .GroupBy(t => t)
            .ToDictionary(g => g.Key, g => g.Count());

        foreach (var (termValue, frequency) in termFrequencies)
        {
            var term = await _termRepository
                .GetByValueAsync(termValue, ct);

            if (term is null)
            {
                term = Term.Create(termValue);
                term.IncrementDocumentFrequency();
                await _termRepository.AddAsync(term, ct);
            }
            else
            {
                term.IncrementDocumentFrequency();
                await _termRepository.UpdateAsync(term, ct);
            }

            var entry = IndexEntry.Create(
                term.Id,
                document.Id,
                frequency);

            await _indexRepository.AddAsync(entry, ct);
        }
    }

    public async Task ReindexDocumentAsync(
        Document document,
        CancellationToken ct)
    {
        await _indexRepository.DeleteByDocumentIdAsync(
            document.Id,
            ct);

        await IndexDocumentAsync(document, ct);
    }
}
