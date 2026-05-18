using MediatR;
using SearchEngine.Domain.Search;
using SearchEngine.Domain.Search.Responses;

namespace SearchEngine.Application.Search.Queries;

public sealed record SearchDocumentsQuery(
    string Query,
    OperatorType Operator,
    string? Category,
    string? Author,
    int Page,
    int PageSize)
    : IRequest<PagedSearchResponse>;
