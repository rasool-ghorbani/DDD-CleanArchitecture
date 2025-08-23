namespace Domain.Aggregates.Customer.Services
{
    public interface ICustomerUniquenessCheckerService
    {
        Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> IsPersonalInfoUniqueAsync(string firstName, string lastName, DateOnly dateOfBirth, CancellationToken cancellationToken = default);
    }
}
