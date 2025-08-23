namespace Domain.SeedWork.Rules
{
    public interface IBusinessRule
    {
        Task<bool> IsBrokenAsync();
        string Message { get; }
    }
}
