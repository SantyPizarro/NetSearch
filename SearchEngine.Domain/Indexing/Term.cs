using SearchEngine.Domain.Common;

namespace SearchEngine.Domain.Indexing;

public sealed class Term : Entity<TermId>
{
    public string Value { get; private set; } = default!;
    public int DocumentFrequency { get; private set; }

    private Term() { } // EF

    private Term(TermId id, string value)
    {
        Id = id;
        Value = value;
        DocumentFrequency = 0;
    }

    public static Term Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Term cannot be empty.");

        return new Term(
            TermId.CreateUnique(),
            value.Trim().ToLowerInvariant()
        );
    }

    public void IncrementDocumentFrequency()
    {
        DocumentFrequency++;
    }

    public void DecrementDocumentFrequency()
    {
        if (DocumentFrequency > 0)
            DocumentFrequency--;
    }
}
