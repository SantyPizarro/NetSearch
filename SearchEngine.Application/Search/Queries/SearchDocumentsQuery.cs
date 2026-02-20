using MediatR;
using SearchEngine.Domain.Search;

namespace SearchEngine.Application.Search.Queries;

public sealed record SearchDocumentsQuery(
    string Query,
    OperatorType Operator = OperatorType.Or
) : IRequest<IReadOnlyCollection<SearchResult>>;