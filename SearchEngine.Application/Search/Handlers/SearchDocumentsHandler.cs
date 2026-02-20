using SearchEngine.Application.Abstractions.Services;
using SearchEngine.Application.Search.Queries;
using SearchEngine.Domain.Search;

namespace SearchEngine.Application.Search.Handlers;

public sealed class SearchDocumentsHandler
{
    private readonly ISearchService _searchService;
    private readonly ITokenizer _tokenizer;

    public SearchDocumentsHandler(
        ISearchService searchService,
        ITokenizer tokenizer)
    {
        _searchService = searchService;
        _tokenizer = tokenizer;
    }

    public async Task<IReadOnlyCollection<SearchResult>> Handle(
        SearchDocumentsQuery query,
        CancellationToken cancellationToken)
    {
        var terms = _tokenizer.Tokenize(query.Query);

        var operatorType = query.Operator.ToUpper() switch
        {
            "OR" => OperatorType.Or,
            _ => OperatorType.And
        };

        var searchQuery = new SearchQuery(
            query.Query,
            terms,
            operatorType);

        return await _searchService.SearchAsync(searchQuery, cancellationToken);
    }
}