namespace SearchEngine.Api.Contracts.Requests;

public sealed record SearchRequest(
    string Query,
    string Operator = "AND");