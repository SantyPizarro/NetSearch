using SearchEngine.Domain.Common;

public sealed class Metadata : ValueObject
{
    public string? Author { get; private set; }
    public string? Category { get; private set; }

    private Metadata() { }

    private Metadata(string? author, string? category)
    {
        Author = author;
        Category = category;
    }

    public static Metadata Create(string? author, string? category)
    {
        return new Metadata(
            author?.Trim(),
            category?.Trim()
        );
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Author ?? string.Empty;
        yield return Category ?? string.Empty;
    }
}