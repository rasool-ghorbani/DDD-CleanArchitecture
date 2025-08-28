using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.ValueObjects;
using FluentAssertions;
using Persistence.Context;
using Persistence.Repositories;
using SharedTest.Fixtures;
using SharedTest.Fixtures.FakeServices;

namespace UnitTests.Infrastructure.Persistence.Repositories
{
    public class CustomerRepositoryTests : IClassFixture<TestDatabaseFixture>
    {
        private readonly ApplicationDbContext _context;
        private readonly FakeCustomerUniquenessChecker _checker;

        public CustomerRepositoryTests(TestDatabaseFixture dbFixture)
        {
            _context = dbFixture.Context;
            _checker = new FakeCustomerUniquenessChecker();
        }

        private async Task<Customer> CreateSampleCustomer(ApplicationDbContext context)
        {
            var customer = await Customer.CreateAsync(
                FirstName.Create("Ali"),
                LastName.Create("Rezayi"),
                DateOfBirth.Create(DateOnly.FromDateTime(DateTime.Today.AddYears(-25))),
                PhoneNumber.Create("+989121234567"),
                Email.Create("ali@test.com"),
                BankAccountNumber.Create("1234567890"),
                _checker);

            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            return customer;
        }

        [Fact]
        public async Task AddAsync_Should_Add_Customer_And_Return_Id()
        {
            var repository = new CustomerRepository(_context);

            var customer = await Customer.CreateAsync(
                FirstName.Create("Reza"),
                LastName.Create("Ahmadi"),
                DateOfBirth.Create(DateOnly.FromDateTime(DateTime.Today.AddYears(-30))),
                PhoneNumber.Create("+989121111111"),
                Email.Create("reza@test.com"),
                BankAccountNumber.Create("9876543210"),
                _checker);

            var id = await repository.AddAsync(customer);

            id.Should().Be(customer.Id);
            (await _context.Customers.FindAsync(id)).Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Customer_If_Exists_And_Not_Deleted()
        {
            var customer = await CreateSampleCustomer(_context);
            var repository = new CustomerRepository(_context);

            var result = await repository.GetByIdAsync(customer.Id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(customer.Id);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_If_Deleted()
        {
            var customer = await CreateSampleCustomer(_context);
            customer.Delete();
            await _context.SaveChangesAsync();

            var repository = new CustomerRepository(_context);

            var result = await repository.GetByIdAsync(customer.Id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByEmailAsync_Should_Return_Customer()
        {
            var customer = await CreateSampleCustomer(_context);
            var repository = new CustomerRepository(_context);

            var result = await repository.GetByEmailAsync("ali@test.com");

            result.Should().NotBeNull();
            result!.Email.Value.Should().Be("ali@test.com");
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_Paginated_Customers()
        {
            for (int i = 1; i <= 10; i++)
            {
                var customer = await Customer.CreateAsync(
                     FirstName.Create("First" + i),
                     LastName.Create("Last" + i),
                     DateOfBirth.Create(DateOnly.FromDateTime(DateTime.Today.AddYears(-25 - i))),
                     PhoneNumber.Create("+98912" + i.ToString("D7")),
                     Email.Create($"test{i}@example.com"),
                     BankAccountNumber.Create((1234567866555490 + i).ToString()),
                     _checker);

                _context.Customers.Add(customer);

            }

            await _context.SaveChangesAsync();

            var repository = new CustomerRepository(_context);

            var page1 = await repository.GetAllAsync(1, 5);
            var page2 = await repository.GetAllAsync(2, 5);

            page1.Count.Should().Be(5);
            page2.Count.Should().Be(5);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Customer()
        {
            var customer = await CreateSampleCustomer(_context);
            var repository = new CustomerRepository(_context);

            customer.Restore(); // مثال تغییر وضعیت
            await repository.UpdateAsync(customer);

            var updated = await _context.Customers.FindAsync(customer.Id);
            updated!.IsDeleted.Should().BeFalse();
        }
    }
}
