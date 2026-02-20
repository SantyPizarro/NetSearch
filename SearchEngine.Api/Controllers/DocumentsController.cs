using MediatR;
using Microsoft.AspNetCore.Mvc;
using SearchEngine.Api.Contracts.Requests;

[ApiController]
[Route("api/documents")]
public class DocumentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        return Ok();
    }


    [HttpPost]
    public async Task<IActionResult> Create(
        CreateDocumentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateDocumentCommand(
            request.Title,
            request.Content,
            request.Tags,
            request.Author,
            request.Category);

        var id = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            id);
    }
}