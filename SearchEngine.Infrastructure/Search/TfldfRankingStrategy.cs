using SearchEngine.Application.Abstractions.Services;
using SearchEngine.Domain.Indexing;

namespace SearchEngine.Infrastructure.Search;

public sealed class TfIdfRankingStrategy : IRankingStrategy
{
    public double CalculateScore(
        Term term,
        int termFrequency,
        int totalDocuments)
    {
        if (totalDocuments == 0)
            return 0;

        if (term.DocumentFrequency == 0)
            return 0;

        // TF
        double tf = termFrequency;

        // IDF
        double idf = Math.Log(
            (double)totalDocuments / term.DocumentFrequency);

        return tf * idf;
    }
}
