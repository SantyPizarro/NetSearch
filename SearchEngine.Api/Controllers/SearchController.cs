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

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery(Name = "q")] string query,
        [FromQuery] OperatorType operatorType = OperatorType.Or,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query cannot be empty.");

        var result = await _mediator.Send(
            new SearchDocumentsQuery(query, operatorType),
            cancellationToken);

        var response = result.Select(r => new SearchResponse
        {
            DocumentId = r.Document.Id.Value,
            Title = r.Document.Title,
            Score = r.Score
        }).ToList();

        return Ok(response);
    }
}