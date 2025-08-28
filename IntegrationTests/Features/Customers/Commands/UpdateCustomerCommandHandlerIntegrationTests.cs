using Application.Exceptions;
using Application.Features.Customers.Commands.UpdateCustomer;
using Domain.Aggregates.Customer.Events;
using Domain.Aggregates.Customer.ValueObjects;
using Domain.SeedWork.Rules;
using Persistence.Context;
using Persistence.Repositories;
using SharedTest.Fixtures;
using SharedTest.Fixtures.FakeServices;

namespace IntegrationTests.Features.Customers.Commands
{
    public class UpdateCustomerCommandHandlerIntegrationTests : IClassFixture<TestDatabaseFixture>
    {
        private readonly ApplicationDbContext _context;
        private readonly FakeCustomerUniquenessChecker _checker;

        public UpdateCustomerCommandHandlerIntegrationTests(TestDatabaseFixture dbFixture)
        {
            _context = dbFixture.Context;
            _checker = new FakeCustomerUniquenessChecker();
        }

        private UpdateCustomerCommandHandler GetHandler() => new(new CustomerRepository(_context), _checker);

        private async Task<Domain.Aggregates.Customer.Customer> AddCustomerToDbAsync(Domain.Aggregates.Customer.Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        [Fact]
        public async Task Handle_ShouldUpdateCustomerSuccessfully()
        {
            // Arrange
            var customer = await Domain.Aggregates.Customer.Customer.CreateAsync(
                FirstName.Create("Alice"),
                LastName.Create("Smith"),
                DateOfBirth.Create(new DateOnly(1990, 1, 1)),
                PhoneNumber.Create("+989186566156"),
                Email.Create("alice@example.com"),
                BankAccountNumber.Create("65463125649814654"),
                _checker
            );

            await AddCustomerToDbAsync(customer);

            var command = new UpdateCustomerCommand
            (
                customer.Id,
                "AliceUpdated",
                "SmithUpdated",
                 new DateOnly(1991, 2, 2),
                "+989186566150",
                "alice.updated@example.com",
                 "333564654654661654"
            );

            var handler = GetHandler();

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var updated = await _context.Customers.FindAsync(customer.Id);
            Assert.NotNull(updated);
            Assert.Equal("AliceUpdated", updated.FirstName.Value);
            Assert.Equal("SmithUpdated", updated.LastName.Value);
            Assert.Single(updated.DomainEvents.OfType<CustomerUpdatedDomainEvent>());
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFound_WhenCustomerDoesNotExist()
        {
            var command = new UpdateCustomerCommand
            (
                Guid.NewGuid(),
                "Ghost",
                "User",
                new DateOnly(2000, 1, 1),
                "+989186566156",
                "ghost@example.com",
                "65498732165478654"
            );

            var handler = GetHandler();

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowBusinessRuleValidationException_WhenEmailIsDuplicate()
        {
            // Arrange: مشتری اول
            var existing = await Domain.Aggregates.Customer.Customer.CreateAsync(
                FirstName.Create("Bob"),
                LastName.Create("Brown"),
                DateOfBirth.Create(new DateOnly(1980, 5, 5)),
                PhoneNumber.Create("+989186566156"),
                Email.Create("bob@example.com"),
                BankAccountNumber.Create("12345678912345"),
                _checker
            );

            await AddCustomerToDbAsync(existing);

            _checker.AddExistingCustomer(existing);

            // Arrange: مشتری دوم
            var customer = await Domain.Aggregates.Customer.Customer.CreateAsync(
                FirstName.Create("Charlie"),
                LastName.Create("Green"),
                DateOfBirth.Create(new DateOnly(1985, 6, 6)),
                PhoneNumber.Create("+989186566156"),
                Email.Create("charlie@example.com"),
                BankAccountNumber.Create("123456789987456"),
                _checker
            );

            await AddCustomerToDbAsync(customer);

            var command = new UpdateCustomerCommand
            (
                customer.Id,
                "Charlie",
                "Green",
                new DateOnly(1985, 6, 6),
                "+989186566156",
                "bob@example.com", // ایمیل تکراری
                "123456789987456"
            );

            var handler = GetHandler();

            // Act + Assert
            await Assert.ThrowsAsync<BusinessRuleValidationException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenCustomerIsDeleted()
        {
            var customer = await Domain.Aggregates.Customer.Customer.CreateAsync(
                FirstName.Create("Deleted"),
                LastName.Create("User"),
                DateOfBirth.Create(new DateOnly(1999, 9, 9)),
                PhoneNumber.Create("+989186566156"),
                Email.Create("deleted@example.com"),
                BankAccountNumber.Create("123456789987456"),
                _checker
            );

            customer.Delete();

            await AddCustomerToDbAsync(customer);

            var command = new UpdateCustomerCommand
            (
                customer.Id,
                "DeletedUpdated",
                "UserUpdated",
                new DateOnly(2000, 10, 10),
                "+989186566156",
                "deleted.updated@example.com",
                "123456789987456"
            );

            var handler = GetHandler();

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}
