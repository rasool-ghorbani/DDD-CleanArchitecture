using Application.Features.Customers.Commands.CreateCustomer;
using Domain.Aggregates.Customer.Events;
using Domain.Aggregates.Customer.ValueObjects;
using Domain.SeedWork.Rules;
using Persistence.Context;
using Persistence.Repositories;
using SharedTest.Fixtures;
using SharedTest.Fixtures.FakeServices;

namespace IntegrationTests.Features.Customers.Commands
{
    public class CreateCustomerCommandHandlerIntegrationTests : IClassFixture<TestDatabaseFixture>
    {
        private readonly ApplicationDbContext _context;
        private readonly FakeCustomerUniquenessChecker _checker;

        public CreateCustomerCommandHandlerIntegrationTests(TestDatabaseFixture dbFixture)
        {
            _context = dbFixture.Context;
            _checker = new FakeCustomerUniquenessChecker();
        }

        private CreateCustomerCommandHandler GetHandler() => new(new CustomerRepository(_context), _checker);

        [Fact]
        public async Task Handle_ShouldCreateCustomerSuccessfully()
        {
            var command = new CreateCustomerCommand
            (
                "Alice",
                "Johnson",
                new DateOnly(1990, 1, 1),
                "+989186566156",
                "alice@example.com",
                "11154654414665333"
            );

            var handler = GetHandler();
            var customerId = await handler.Handle(command, CancellationToken.None);

            var customer = await _context.Customers.FindAsync(customerId);

            Assert.NotNull(customer);
            Assert.Equal("Alice", customer.FirstName.Value);
            Assert.Single(customer.DomainEvents.OfType<CustomerCreatedDomainEvent>());
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenEmailIsDuplicate()
        {
            var existing = await Domain.Aggregates.Customer.Customer.CreateAsync(
                FirstName.Create("Bob"),
                LastName.Create("Smith"),
                DateOfBirth.Create(new DateOnly(1990, 1, 1)),
                PhoneNumber.Create("+989186566156"),
                Email.Create("bob@example.com"),
                BankAccountNumber.Create("4685616814564861"),
                _checker
            );

            _checker.AddExistingCustomer(existing);

            var command = new CreateCustomerCommand
            (
                "Charlie",
                "Brown",
                new DateOnly(1995, 5, 5),
                "+989186566156",
                "bob@example.com", // ایمیل تکراری
                "4685616814564861"
            );

            var handler = GetHandler();

            await Assert.ThrowsAsync<BusinessRuleValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenPersonalInfoIsDuplicate()
        {
            var existing = await Domain.Aggregates.Customer.Customer.CreateAsync(
                FirstName.Create("Dana"),
                LastName.Create("White"),
                DateOfBirth.Create(new DateOnly(1985, 3, 3)),
                PhoneNumber.Create("+989186566156"),
                Email.Create("dana@example.com"),
                BankAccountNumber.Create("4685616814564861"),
                _checker
            );
            _checker.AddExistingCustomer(existing);

            var command = new CreateCustomerCommand
            (
                "Dana",
                "White",
                new DateOnly(1985, 3, 3), // اطلاعات شخصی تکراری
                "+989186566156",
                "newemail@example.com",
                "4685616814564861"
            );

            var handler = GetHandler();

            await Assert.ThrowsAsync<BusinessRuleValidationException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }
    }
}
