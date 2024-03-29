using MediatR;

namespace iPractice.SharedKernel.BaseClasses
{
    public abstract class BaseDomainEvent : INotification
    {
        public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;

    }
}
