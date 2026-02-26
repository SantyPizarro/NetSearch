using MediatR;
using static System.Net.Mime.MediaTypeNames;

csharp SearchEngine.Application\Documents\Commands\UpdateDocumentCommand.cs
using System;
using MediatR;

public sealed record UpdateDocumentCommand(
    Guid Id,
    string Title,
    string Content,
    IEnumerable<string> Tags,
    string? Author,
    string? Category
) : IRequest<Unit>;