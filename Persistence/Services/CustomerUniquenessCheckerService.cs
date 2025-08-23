using Domain.Aggregates.Customer.Services;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Services
{
    public class CustomerUniquenessCheckerService : ICustomerUniquenessCheckerService
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerUniquenessCheckerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
        {
            return !await _dbContext.Customers.AnyAsync(c => c.Email.Value == email && !c.IsDeleted, cancellationToken);
        }

        public async Task<bool> IsPersonalInfoUniqueAsync(string firstName, string lastName, DateOnly dateOfBirth, CancellationToken cancellationToken = default)
        {
            return !await _dbContext.Customers.AnyAsync(c =>
                c.FirstName.Value == firstName &&
                c.LastName.Value == lastName &&
                c.DateOfBirth.Value == dateOfBirth &&
                !c.IsDeleted, cancellationToken);
        }
    }
}
