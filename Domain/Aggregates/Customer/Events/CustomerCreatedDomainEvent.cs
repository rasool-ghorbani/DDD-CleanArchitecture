using Domain.SeedWork.Events;

namespace Domain.Aggregates.Customer.Events
{
    public sealed class CustomerCreatedDomainEvent : DomainEventBase
    {
        public Guid CustomerId { get; }

        public CustomerCreatedDomainEvent(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}
