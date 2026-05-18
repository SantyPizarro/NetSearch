using SearchEngine.Domain.Common;
using SearchEngine.Domain.Documents;

namespace SearchEngine.Domain.Indexing;

public sealed class IndexEntry : Entity<Guid>
{
    public TermId TermId { get; private set; } = default!;
    public DocumentId DocumentId { get; private set; } = default!;
    public int TermFrequency { get; private set; }

    private IndexEntry() { } // EF

    private IndexEntry(
        Guid id,
        TermId termId,
        DocumentId documentId,
        int termFrequency)
    {
        Id = id;
        TermId = termId;
        DocumentId = documentId;
        TermFrequency = termFrequency;
    }

    public static IndexEntry Create(
        TermId termId,
        DocumentId documentId,
        int termFrequency)
    {
        if (termFrequency <= 0)
            throw new ArgumentException("Term frequency must be greater than zero.");

        return new IndexEntry(
            Guid.NewGuid(),
            termId,
            documentId,
            termFrequency
        );
    }

    public void UpdateFrequency(int newFrequency)
    {
        if (newFrequency <= 0)
            throw new ArgumentException("Term frequency must be positive.");

        TermFrequency = newFrequency;
    }
}
