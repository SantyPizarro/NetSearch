using SearchEngine.Domain.Indexing;

namespace SearchEngine.Application.Abstractions.Services;

public interface IRankingStrategy
{
    double CalculateScore(
        Term term,
        int termFrequency,
        int totalDocuments);
}
