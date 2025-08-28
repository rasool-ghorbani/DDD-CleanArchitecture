using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.Services;

namespace SharedTest.Fixtures.FakeServices
{
    public class FakeCustomerUniquenessChecker : ICustomerUniquenessCheckerService
    {
        private readonly List<Customer> _existingCustomers = new();

        public Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default) =>
            Task.FromResult(!_existingCustomers.Any(c => c.Email.Value == email));

        public Task<bool> IsPersonalInfoUniqueAsync(string firstName, string lastName, DateOnly dateOfBirth, CancellationToken cancellationToken = default) =>
            Task.FromResult(!_existingCustomers.Any(c =>
                c.FirstName.Value == firstName &&
                c.LastName.Value == lastName &&
                c.DateOfBirth.Value == dateOfBirth));

        public void AddExistingCustomer(Customer customer) =>
            _existingCustomers.Add(customer);
    }
}
