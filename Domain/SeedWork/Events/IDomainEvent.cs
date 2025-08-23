namespace Domain.SeedWork.Events
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
