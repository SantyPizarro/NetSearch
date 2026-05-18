using SearchEngine.Application.Abstractions.Services;
using SearchEngine.Application.Search.Handlers;
using SearchEngine.Application.Search.Queries;
using SearchEngine.Domain.Documents;
using SearchEngine.Domain.Search;

namespace SearchEngine.Tests;

public sealed class SearchDocumentsHandlerTests
{
    [Fact]
    public async Task Handle_FiltersByMetadataPaginatesAndBuildsSnippet()
    {
        var first = Document.Create(
            "First",
            "The document contains a very specific needle inside its content.",
            Array.Empty<string>(),
            "Ada",
            "Tech");

        var second = Document.Create(
            "Second",
            "Another needle result for page two.",
            Array.Empty<string>(),
            "Ada",
            "Tech");

        var ignored = Document.Create(
            "Ignored",
            "Needle exists here but category is different.",
            Array.Empty<string>(),
            "Ada",
            "Notes");

        var handler = new SearchDocumentsHandler(
            new StubSearchService(new[]
            {
                new SearchResult(first, 2),
                new SearchResult(second, 1),
                new SearchResult(ignored, 3)
            }),
            new SplitTokenizer());

        var response = await handler.Handle(
            new SearchDocumentsQuery("needle", OperatorType.Or, "Tech", "Ada", 1, 1),
            CancellationToken.None);

        Assert.Equal(2, response.Total);
        Assert.Equal(1, response.Page);
        Assert.Equal(1, response.PageSize);
        var result = Assert.Single(response.Results);
        Assert.Equal(first.Id.Value, result.DocumentId);
        Assert.Contains("<b>needle</b>", result.Snippet, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Handle_ClampsInvalidPagingValues()
    {
        var document = Document.Create(
            "First",
            "Small search content",
            Array.Empty<string>(),
            null,
            null);

        var handler = new SearchDocumentsHandler(
            new StubSearchService(new[] { new SearchResult(document, 1) }),
            new SplitTokenizer());

        var response = await handler.Handle(
            new SearchDocumentsQuery("search", OperatorType.Or, null, null, -5, 250),
            CancellationToken.None);

        Assert.Equal(1, response.Page);
        Assert.Equal(100, response.PageSize);
        Assert.Single(response.Results);
    }

    private sealed class StubSearchService : ISearchService
    {
        private readonly IReadOnlyCollection<SearchResult> _results;

        public StubSearchService(IReadOnlyCollection<SearchResult> results)
        {
            _results = results;
        }

        public Task<IReadOnlyCollection<SearchResult>> SearchAsync(
            SearchQuery query,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_results);
        }
    }

    private sealed class SplitTokenizer : ITokenizer
    {
        public IReadOnlyCollection<string> Tokenize(string text)
        {
            return text
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(x => x.ToLowerInvariant())
                .ToList();
        }
    }
}
