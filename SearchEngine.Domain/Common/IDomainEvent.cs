using MediatR;

namespace SearchEngine.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}