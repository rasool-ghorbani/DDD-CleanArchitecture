using Application.Exceptions;
using Application.Features.Customers.Queries.GetCustomerById;
using AutoMapper;
using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.ValueObjects;
using Persistence.Context;
using Persistence.Repositories;
using SharedTest.Fixtures;
using SharedTest.Fixtures.FakeServices;

namespace IntegrationTests.Features.Customers.Queries
{
    public class GetCustomerByIdQueryHandlerTests : IClassFixture<TestDatabaseFixture>, IClassFixture<MapperFixture>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly FakeCustomerUniquenessChecker _checker;

        public GetCustomerByIdQueryHandlerTests(TestDatabaseFixture dbFixture, MapperFixture mapperFixture)
        {
            _context = dbFixture.Context;
            _mapper = mapperFixture.Mapper;
            _checker = new FakeCustomerUniquenessChecker();
        }

        private GetCustomerByIdQueryHandler GetHandler() => new(new CustomerRepository(_context), _mapper);

        private async Task<Customer> AddCustomerToDbAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        [Fact]
        public async Task Handle_ShouldReturnCustomerDto_WhenCustomerExists()
        {
            var customer = await Customer.CreateAsync(
                FirstName.Create("Ali"),
                LastName.Create("Rezaei"),
                DateOfBirth.Create(new DateOnly(1990, 1, 1)),
                PhoneNumber.Create("+989123456789"),
                Email.Create("ali.rezaei@example.com"),
                BankAccountNumber.Create("123456789012345"),
                _checker
            );

            await AddCustomerToDbAsync(customer);

            var handler = GetHandler();

            var query = new GetCustomerByIdQuery { CustomerId = customer.Id };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(customer.Id, result.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenCustomerDoesNotExist()
        {
            // Arrange
            var query = new GetCustomerByIdQuery { CustomerId = Guid.NewGuid() };

            var handler = GetHandler();

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldMapAllFieldsCorrectly_WhenCustomerExists()
        {
            // Arrange
            var customer = await Customer.CreateAsync(
                FirstName.Create("Sara"),
                LastName.Create("Ahmadi"),
                DateOfBirth.Create(new DateOnly(1985, 5, 5)),
                PhoneNumber.Create("+989112223334"),
                Email.Create("sara.ahmadi@example.com"),
                BankAccountNumber.Create("987654321098765"),
                _checker
            );

            await AddCustomerToDbAsync(customer);

            var handler = GetHandler();

            var query = new GetCustomerByIdQuery { CustomerId = customer.Id };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customer.Id, result.Id);
            Assert.Equal(customer.FirstName.Value, result.FirstName);
            Assert.Equal(customer.LastName.Value, result.LastName);
            Assert.Equal(customer.PhoneNumber.ToString(), result.PhoneNumber);
            Assert.Equal(customer.Email.Value, result.Email);
            Assert.Equal(customer.BankAccountNumber.Value, result.BankAccountNumber);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFound_WhenCustomerIsDeleted()
        {
            // Arrange
            var customer = await Customer.CreateAsync(
                FirstName.Create("Deleted"),
                LastName.Create("User"),
                DateOfBirth.Create(new DateOnly(1999, 9, 9)),
                PhoneNumber.Create("+989000000000"),
                Email.Create("deleted@example.com"),
                BankAccountNumber.Create("111222333444"),
               _checker
            );
            customer.Delete();

            await AddCustomerToDbAsync(customer);

            var handler = GetHandler();
            var query = new GetCustomerByIdQuery { CustomerId = customer.Id };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}