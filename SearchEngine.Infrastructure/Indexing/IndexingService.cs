using SearchEngine.Application.Abstractions.Persistence;
using SearchEngine.Application.Abstractions.Services;
using SearchEngine.Domain.Documents;
using SearchEngine.Domain.Indexing;

namespace SearchEngine.Infrastructure.Indexing;

public sealed class IndexingService : IIndexingService
{
    private readonly ITermRepository _termRepository;
    private readonly IIndexRepository _indexRepository;
    private readonly ITokenizer _tokenizer;
    private readonly IUnitOfWork _unitOfWork;

    public IndexingService(
        ITermRepository termRepository,
        IIndexRepository indexRepository,
        ITokenizer tokenizer,
        IUnitOfWork unitOfWork)
    {
        _termRepository = termRepository;
        _indexRepository = indexRepository;
        _tokenizer = tokenizer;
        _unitOfWork = unitOfWork;
    }

    public async Task IndexAsync(
        Document document,
        CancellationToken cancellationToken = default)
    {
        await _indexRepository.DeleteByDocumentIdAsync(
            document.Id,
            cancellationToken);

        var tokens = _tokenizer.Tokenize(document.Content);

        if (tokens.Count == 0)
            return;

        var termFrequencies = tokens
            .GroupBy(t => t)
            .ToDictionary(g => g.Key, g => g.Count());

        foreach (var (termValue, frequency) in termFrequencies)
        {
            var term = await _termRepository
                .GetByValueAsync(termValue, cancellationToken);

            if (term is null)
            {
                term = Term.Create(termValue);
                await _termRepository.AddAsync(term, cancellationToken);
            }

            term.IncrementDocumentFrequency();
            await _termRepository.UpdateAsync(term, cancellationToken);

            var entry = IndexEntry.Create(
                term.Id,
                document.Id,
                frequency);

            await _indexRepository.AddAsync(entry, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}