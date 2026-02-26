using MediatR;
using SearchEngine.Domain.Search;

public sealed record SearchDocumentsQuery(
    string Query,
    OperatorType Operator,
    string? Category,
    string? Author,
    int Page,
    int PageSize)
    : IRequest<PagedSearchResponse>;