using MediatR;
using Microsoft.AspNetCore.Mvc;
using SearchEngine.Api.Contracts.Requests;
using SearchEngine.Application.Search.Queries;

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

        if (request.Page < 1)
            return BadRequest("Page must be greater than or equal to 1.");

        if (request.PageSize is < 1 or > 100)
            return BadRequest("PageSize must be between 1 and 100.");

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
