using Domain.SeedWork.Events;

namespace Domain.Aggregates.Customer.Events
{
    public sealed class CustomerRestoredDomainEvent : DomainEventBase
    {
        public Guid CustomerId { get; }

        public CustomerRestoredDomainEvent(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}
