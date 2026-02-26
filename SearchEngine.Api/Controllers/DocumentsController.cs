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

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var documents = await _mediator.Send(
            new GetAllDocumentsQuery(),
            cancellationToken);

        return Ok(documents);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var document = await _mediator.Send(
            new GetDocumentByIdQuery(id),
            cancellationToken);

        if (document is null)
            return NotFound();

        return Ok(document);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateDocumentRequest request,
        CancellationToken cancellationToken)
    {
        var id = await _mediator.Send(
            new CreateDocumentCommand(
                request.Title,
                request.Content,
                request.Tags,
                request.Author,
                request.Category),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateDocumentRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new UpdateDocumentCommand(
                id,
                request.Title,
                request.Content,
                request.Tags,
                request.Author,
                request.Category),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteDocumentCommand(id),
            cancellationToken);

        return NoContent();
    }
}