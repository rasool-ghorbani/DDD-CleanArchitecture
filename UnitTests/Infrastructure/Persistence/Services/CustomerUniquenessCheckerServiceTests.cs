using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.ValueObjects;
using FluentAssertions;
using Persistence.Context;
using Persistence.Services;
using SharedTest.Fixtures;
using SharedTest.Fixtures.FakeServices;

namespace UnitTests.Infrastructure.Persistence.Services
{
    public class CustomerUniquenessCheckerServiceTests : IClassFixture<TestDatabaseFixture>
    {
        private readonly ApplicationDbContext _context;
        private readonly FakeCustomerUniquenessChecker _checker;

        public CustomerUniquenessCheckerServiceTests(TestDatabaseFixture dbFixture)
        {
            _context = dbFixture.Context;
            _checker = new FakeCustomerUniquenessChecker();
        }

        [Fact]
        public async Task IsEmailUniqueAsync_Should_Return_False_If_Email_Exists()
        {
            // Arrange

            var customer = await Customer.CreateAsync(
                FirstName.Create("Ali"),
                LastName.Create("Rezayi"),
                DateOfBirth.Create(DateOnly.FromDateTime(DateTime.Today.AddYears(-25))),
                PhoneNumber.Create("+989121234567"),
                Email.Create("ali@test.com"),
                BankAccountNumber.Create("1234567890"),
                _checker); // فقط برای ساخت

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var service = new CustomerUniquenessCheckerService(_context);

            // Act
            var result = await service.IsEmailUniqueAsync("ali@test.com");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsEmailUniqueAsync_Should_Return_True_If_Email_Not_Exists()
        {
            var service = new CustomerUniquenessCheckerService(_context);

            var result = await service.IsEmailUniqueAsync("new@test.com");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsPersonalInfoUniqueAsync_Should_Return_False_If_PersonalInfo_Exists()
        {
            var customer = await Customer.CreateAsync(
                FirstName.Create("Ali"),
                LastName.Create("Rezayi"),
                DateOfBirth.Create(DateOnly.FromDateTime(DateTime.Today.AddYears(-25))),
                PhoneNumber.Create("+989121234567"),
                Email.Create("ali@test.com"),
                BankAccountNumber.Create("1234567890"),
                _checker);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var service = new CustomerUniquenessCheckerService(_context);

            var result = await service.IsPersonalInfoUniqueAsync("Ali", "Rezayi", DateOnly.FromDateTime(DateTime.Today.AddYears(-25)));

            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsPersonalInfoUniqueAsync_Should_Return_True_If_PersonalInfo_Not_Exists()
        {
            var service = new CustomerUniquenessCheckerService(_context);

            var result = await service.IsPersonalInfoUniqueAsync("Reza", "Ahmadi", DateOnly.FromDateTime(DateTime.Today.AddYears(-30)));

            result.Should().BeTrue();
        }
    }
}