namespace SearchEngine.Domain.Search;

public sealed class SearchQuery
{
    public string RawQuery { get; }
    public IReadOnlyCollection<string> Terms { get; }
    public OperatorType Operator { get; }

    public SearchQuery(string rawQuery, IEnumerable<string> terms, OperatorType @operator)
    {
        RawQuery = rawQuery;
        Terms = terms.ToList().AsReadOnly();
        Operator = @operator;
    }
}