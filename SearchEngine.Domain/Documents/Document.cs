using SearchEngine.Domain.Common;
using SearchEngine.Domain.Events;

namespace SearchEngine.Domain.Documents;

public sealed class Document : Entity<DocumentId>
{
    private readonly List<Tag> _tags = new();

    public string Title { get; private set; } = default!;
    public string Content { get; private set; } = default!;
    public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();
    public Metadata Metadata { get; private set; } = default!;

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Document() { } // For EF Core

    private Document(
        DocumentId id,
        string title,
        string content,
        IEnumerable<Tag> tags,
        Metadata metadata)
    {
        Id = id;
        Title = title;
        Content = content;
        Metadata = metadata;

        _tags = tags.Distinct().ToList();

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Document Create(
        string title,
        string content,
        IEnumerable<string> tags,
        string? author,
        string? category)
    {
        Validate(title, content);

        var tagObjects = tags.Select(Tag.Create);

        var document = new Document(
            DocumentId.CreateUnique(),
            title.Trim(),
            content.Trim(),
            tagObjects,
            Metadata.Create(author, category)
        );

        document.AddDomainEvent(new DocumentCreatedEvent(document.Id));

        return document;
    }

    public void Update(
        string title,
        string content,
        IEnumerable<string> tags,
        string? author,
        string? category)
    {
        Validate(title, content);

        Title = title.Trim();
        Content = content.Trim();
        Metadata = Metadata.Create(author, category);

        _tags.Clear();
        _tags.AddRange(tags.Select(Tag.Create).Distinct());

        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new DocumentUpdatedEvent(Id));
    }

    public void AddTag(string tag)
    {
        var tagObj = Tag.Create(tag);

        if (_tags.Contains(tagObj))
            return;

        _tags.Add(tagObj);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveTag(string tag)
    {
        var tagObj = Tag.Create(tag);

        _tags.Remove(tagObj);
        UpdatedAt = DateTime.UtcNow;
    }

    private static void Validate(string title, string content)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.");

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Content cannot be empty.");
    }

    public void MarkAsDeleted()
    {
        AddDomainEvent(new DocumentDeletedEvent(Id));
    }
}
