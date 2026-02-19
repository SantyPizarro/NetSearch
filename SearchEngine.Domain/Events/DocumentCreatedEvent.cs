using SearchEngine.Domain.Common;
using SearchEngine.Domain.Documents;

namespace SearchEngine.Domain.Events;

public sealed record DocumentCreatedEvent(DocumentId DocumentId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
