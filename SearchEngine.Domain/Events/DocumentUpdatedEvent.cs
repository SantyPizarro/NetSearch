using SearchEngine.Domain.Common;
using SearchEngine.Domain.Documents;

namespace SearchEngine.Domain.Events;

public sealed record DocumentUpdatedEvent(DocumentId DocumentId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
