using SearchEngine.Domain.Search;

namespace SearchEngine.Application.Abstractions.Services;

public interface ISearchService
{
    Task<IReadOnlyCollection<SearchResult>> SearchAsync(SearchQuery query, CancellationToken cancellationToken = default);
}