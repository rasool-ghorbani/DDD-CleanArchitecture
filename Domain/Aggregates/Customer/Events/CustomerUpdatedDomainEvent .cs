using Domain.SeedWork.Events;

namespace Domain.Aggregates.Customer.Events
{
    public sealed class CustomerUpdatedDomainEvent : DomainEventBase
    {
        public Guid CustomerId { get; }

        public CustomerUpdatedDomainEvent(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}
