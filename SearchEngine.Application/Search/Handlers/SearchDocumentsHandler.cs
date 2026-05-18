using MediatR;
using SearchEngine.Application.Abstractions.Services;
using SearchEngine.Application.Search.Queries;
using SearchEngine.Domain.Search;
using SearchEngine.Domain.Search.Responses;

namespace SearchEngine.Application.Search.Handlers;

public sealed class SearchDocumentsHandler
    : IRequestHandler<SearchDocumentsQuery, PagedSearchResponse>
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

    public async Task<PagedSearchResponse> Handle(
        SearchDocumentsQuery request,
        CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var terms = _tokenizer.Tokenize(request.Query);

        var searchQuery = new SearchQuery(
            request.Query,
            terms,
            request.Operator);

        var results = (await _searchService.SearchAsync(searchQuery, cancellationToken)).ToList();

        if (!string.IsNullOrWhiteSpace(request.Category))
            results = results
                .Where(r => string.Equals(r.Document.Metadata.Category, request.Category, StringComparison.OrdinalIgnoreCase))
                .ToList();

        if (!string.IsNullOrWhiteSpace(request.Author))
            results = results
                .Where(r => string.Equals(r.Document.Metadata.Author, request.Author, StringComparison.OrdinalIgnoreCase))
                .ToList();

        var total = results.Count;

        var paged = results
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var mapped = paged.Select(r => new SearchResponse
        {
            DocumentId = r.Document.Id.Value,
            Title = r.Document.Title,
            Score = r.Score,
            Snippet = BuildSnippet(r.Document.Content, request.Query)
        }).ToList();

        return new PagedSearchResponse
        {
            Total = total,
            Page = page,
            PageSize = pageSize,
            Results = mapped
        };
    }

    private static string BuildSnippet(string content, string query)
    {
        if (string.IsNullOrWhiteSpace(content))
            return string.Empty;

        var terms = query
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var term in terms)
        {
            var index = content
                .IndexOf(term, StringComparison.OrdinalIgnoreCase);

            if (index >= 0)
            {
                var start = Math.Max(index - 40, 0);
                var length = Math.Min(120, content.Length - start);

                var snippet = content.Substring(start, length);

                return snippet.Replace(
                    term,
                    $"<b>{term}</b>",
                    StringComparison.OrdinalIgnoreCase);
            }
        }

        return content.Length > 120
            ? content.Substring(0, 120)
            : content;
    }
}
