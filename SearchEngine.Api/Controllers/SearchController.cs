using MediatR;
using Microsoft.AspNetCore.Mvc;
using SearchEngine.Api.Contracts.Responses;
using SearchEngine.Application.Search.Queries;
using SearchEngine.Domain.Search;

namespace SearchEngine.Api.Controllers;

[ApiController]
[Route("api/search")]
public sealed class SearchController : ControllerBase
{
    private readonly IMediator _mediator;

    public SearchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Search(
        [FromBody] SearchRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
            return BadRequest("Query cannot be empty.");

        var result = await _mediator.Send(
            new SearchDocumentsQuery(
                request.Query,
                request.Operator,
                request.Category,
                request.Author,
                request.Page,
                request.PageSize),
            cancellationToken);

        return Ok(result);
    }
}