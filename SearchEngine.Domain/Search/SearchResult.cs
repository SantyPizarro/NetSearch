using SearchEngine.Domain.Documents;

namespace SearchEngine.Domain.Search;

public sealed class SearchResult
{
    public Document Document { get; }
    public double Score { get; }

    public SearchResult(Document document, double score)
    {
        Document = document;
        Score = score;
    }
}