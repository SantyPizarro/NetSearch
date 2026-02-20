namespace SearchEngine.Application.Search.Queries;

public sealed record SearchDocumentsQuery(
    string Query,
    string Operator = "AND");