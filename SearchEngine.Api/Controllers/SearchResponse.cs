using Microsoft.AspNetCore.Mvc;
using SearchEngine.Application.Search.Handlers;
using SearchEngine.Application.Search.Queries;
using SearchEngine.Api.Contracts.Requests;
using SearchEngine.Api.Contracts.Responses;

namespace SearchEngine.Api.Controllers;

[ApiController]
[Route("api/search")]
public sealed class SearchController : ControllerBase
{
    private readonly SearchDocumentsHandler _handler;

    public SearchController(SearchDocumentsHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Search(
        SearchRequest request,
        CancellationToken cancellationToken)
    {
        var query = new SearchDocumentsQuery(
            request.Query,
            request.Operator);

        var results = await _handler.Handle(query, cancellationToken);

        var response = results.Select(x =>
            new SearchResponse(
                x.Document.Id.Value,
                x.Document.Title,
                x.Score));

        return Ok(response);
    }
}