namespace SearchEngine.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
