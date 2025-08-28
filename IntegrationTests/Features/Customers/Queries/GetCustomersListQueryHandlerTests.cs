using Application.Features.Customers.Queries.GetCustomersListQuery;
using AutoMapper;
using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.Repositories;
using Domain.Aggregates.Customer.ValueObjects;
using Persistence.Context;
using Persistence.Repositories;
using SharedTest.Fixtures;
using SharedTest.Fixtures.FakeServices;

namespace IntegrationTests.Features.Customers.Queries
{
    public class GetCustomersListQueryHandlerTests : IClassFixture<TestDatabaseFixture>, IClassFixture<MapperFixture>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        private readonly FakeCustomerUniquenessChecker _checker;

        public GetCustomersListQueryHandlerTests(TestDatabaseFixture dbFixture, MapperFixture mapperFixture)
        {
            _context = dbFixture.Context;
            _mapper = mapperFixture.Mapper;
            _customerRepository = new CustomerRepository(_context);
            _checker = new FakeCustomerUniquenessChecker();
        }

        private GetCustomersListQueryHandler GetHandler() => new(new CustomerRepository(_context), _mapper);


        private async Task<Customer> AddCustomerToDbAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        [Fact]
        public async Task Handle_ShouldReturnAllCustomers_WhenCustomersExist()
        {
            // Arrange
            var customers = new List<Customer>
            {
                await Customer.CreateAsync(
                    FirstName.Create("Ali"),
                    LastName.Create("Rezaei"),
                    DateOfBirth.Create(new DateOnly(1990,1,1)),
                    PhoneNumber.Create("+989123456789"),
                    Email.Create("ali@example.com"),
                    BankAccountNumber.Create("12345678912343"),
                   _checker
                ),
                await Customer.CreateAsync(
                    FirstName.Create("Sara"),
                    LastName.Create("Ahmadi"),
                    DateOfBirth.Create(new DateOnly(1995,5,5)),
                    PhoneNumber.Create("+989112233445"),
                    Email.Create("sara@example.com"),
                    BankAccountNumber.Create("12345678912343"),
                   _checker
                )
            };

            foreach (var c in customers)
                await AddCustomerToDbAsync(c);

            var query = new GetCustomersListQuery { Page = 1, PageSize = 10 };

            var handler = GetHandler();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, x => x.Email == "ali@example.com");
            Assert.Contains(result, x => x.Email == "sara@example.com");
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoCustomersExist()
        {
            // Arrange
            var query = new GetCustomersListQuery { Page = 1, PageSize = 10 };

            var handler = GetHandler();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_ShouldRespectPagingParameters()
        {
            // Arrange
            for (int i = 0; i < 5; i++)
            {
                var customer = await Customer.CreateAsync(
                    FirstName.Create($"First{i}"),
                    LastName.Create($"Last{i}"),
                    DateOfBirth.Create(new DateOnly(1990, 1, 1)),
                    PhoneNumber.Create($"+98918656615{i}"),
                    Email.Create($"customer{i}@example.com"),
                    BankAccountNumber.Create($"12345678912343{i}"),
                    _checker
                );
                await AddCustomerToDbAsync(customer);
            }

            var query = new GetCustomersListQuery { Page = 2, PageSize = 2 };

            var handler = GetHandler();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); // صفحه ۲ با ۲ مشتری
        }
    }

}
