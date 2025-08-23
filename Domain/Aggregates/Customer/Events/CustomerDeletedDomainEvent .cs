using Domain.SeedWork.Events;

namespace Domain.Aggregates.Customer.Events
{
    public sealed class CustomerDeletedDomainEvent : DomainEventBase
    {
        public Guid CustomerId { get; }

        public CustomerDeletedDomainEvent(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}
