using Application.Exceptions;
using Application.Features.Customers.Commands.DeleteCustomer;
using Domain.Aggregates.Customer.Events;
using Domain.Aggregates.Customer.ValueObjects;
using Persistence.Context;
using Persistence.Repositories;
using SharedTest.Fixtures;
using SharedTest.Fixtures.FakeServices;

namespace IntegrationTests.Features.Customers.Commands
{
    public class DeleteCustomerCommandHandlerIntegrationTests : IClassFixture<TestDatabaseFixture>
    {
        private readonly ApplicationDbContext _context;
        private readonly FakeCustomerUniquenessChecker _checker;

        public DeleteCustomerCommandHandlerIntegrationTests(TestDatabaseFixture dbFixture)
        {
            _context = dbFixture.Context;
            _checker = new FakeCustomerUniquenessChecker();
        }

        private DeleteCustomerCommandHandler GetHandler() => new(new CustomerRepository(_context));

        private async Task<Domain.Aggregates.Customer.Customer> AddCustomerToDbAsync(Domain.Aggregates.Customer.Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        [Fact]
        public async Task Handle_ShouldDeleteCustomerSuccessfully()
        {
            // Arrange
            var customer = await Domain.Aggregates.Customer.Customer.CreateAsync(
                FirstName.Create("John"),
                LastName.Create("Doe"),
                DateOfBirth.Create(new DateOnly(1990, 1, 1)),
                PhoneNumber.Create("+989186566156"),
                Email.Create("john.doe@example.com"),
                BankAccountNumber.Create("12345678912345"),
                _checker
            );

            await AddCustomerToDbAsync(customer);

            var command = new DeleteCustomerCommand(customer.Id);
            var handler = GetHandler();

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var deleted = await _context.Customers.FindAsync(customer.Id);
            Assert.NotNull(deleted);
            Assert.True(deleted.IsDeleted);
            Assert.Single(deleted.DomainEvents.OfType<CustomerDeletedDomainEvent>());
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFound_WhenCustomerDoesNotExist()
        {
            var command = new DeleteCustomerCommand(Guid.NewGuid());
            var handler = GetHandler();

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldNotAddDuplicateDomainEvent_WhenCustomerAlreadyDeleted()
        {
            // Arrange
            var customer = await Domain.Aggregates.Customer.Customer.CreateAsync(
                FirstName.Create("Jane"),
                LastName.Create("Smith"),
                DateOfBirth.Create(new DateOnly(1995, 5, 5)),
                PhoneNumber.Create("+989186566156"),
                Email.Create("jane.smith@example.com"),
                BankAccountNumber.Create("12345678912345"),
                _checker
            );

            // Act
            customer.Delete(); // اولین بار حذف
            customer.Delete(); // بار دوم حذف

            // Assert
            Assert.True(customer.IsDeleted);
            Assert.Single(customer.DomainEvents.OfType<CustomerDeletedDomainEvent>());
        }
    }
}