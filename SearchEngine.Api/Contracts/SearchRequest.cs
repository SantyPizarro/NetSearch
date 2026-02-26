using SearchEngine.Domain.Search;

public sealed class SearchRequest
{
    public string Query { get; set; } = string.Empty;
    public OperatorType Operator { get; set; } = OperatorType.Or;

    public string? Category { get; set; }
    public string? Author { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}