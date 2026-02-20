using MediatR;
using SearchEngine.Application.Abstractions.Services;
using SearchEngine.Application.Search.Queries;
using SearchEngine.Domain.Search;

namespace SearchEngine.Application.Search.Handlers;

public sealed class SearchDocumentsHandler
    : IRequestHandler<SearchDocumentsQuery, IReadOnlyCollection<SearchResult>>
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
        SearchDocumentsQuery request,
        CancellationToken cancellationToken)
    {
        var terms = _tokenizer.Tokenize(request.Query);

        var searchQuery = new SearchQuery(
            request.Query,
            terms,
            request.Operator);

        return await _searchService.SearchAsync(searchQuery, cancellationToken);
    }
}