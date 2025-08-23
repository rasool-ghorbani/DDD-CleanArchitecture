using Domain.SeedWork.Events;

namespace Domain.SeedWork
{
    public abstract class Entity<TId>
    {
        public TId Id { get; protected set; }
       
        private List<IDomainEvent> _domainEvents = new();
      
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

        protected void AddDomainEvent(IDomainEvent eventItem) => _domainEvents.Add(eventItem);
        
        public void ClearDomainEvents() => _domainEvents.Clear();

        public override bool Equals(object obj)
        {
            if (obj is not Entity<TId> other || GetType() != other.GetType())
                return false;
            return EqualityComparer<TId>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode() => HashCode.Combine(Id);
    }
}
