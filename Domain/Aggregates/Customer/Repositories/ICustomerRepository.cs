namespace Domain.Aggregates.Customer.Repositories
{
    public interface ICustomerRepository
    {
        Task<Guid> AddAsync(Customer customer, CancellationToken cancellationToken = default);
        Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<List<Customer>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);
    }
}