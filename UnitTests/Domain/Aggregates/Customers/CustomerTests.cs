using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.Events;
using Domain.Aggregates.Customer.Services;
using Domain.Aggregates.Customer.ValueObjects;
using FluentAssertions;
using Moq;

namespace UnitTests.Domain.Aggregates.Customers
{
    public class CustomerTests
    {
        private readonly Mock<ICustomerUniquenessCheckerService> _uniquenessChecker;

        public CustomerTests()
        {
            _uniquenessChecker = new Mock<ICustomerUniquenessCheckerService>();

            _uniquenessChecker
                .Setup(x => x.IsEmailUniqueAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(true);

            _uniquenessChecker
                .Setup(x => x.IsPersonalInfoUniqueAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateOnly>(), CancellationToken.None))
                .ReturnsAsync(true);
        }

        private async Task<Customer> CreateSampleCustomerAsync()
        {
            return await Customer.CreateAsync(
                FirstName.Create("Ali"),
                LastName.Create("Rezayi"),
                DateOfBirth.Create(DateOnly.FromDateTime(DateTime.Today.AddYears(-25))),
                PhoneNumber.Create("+989121234567"),
                Email.Create("ali@test.com"),
                BankAccountNumber.Create("1234567890"),
                _uniquenessChecker.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Customer_And_Raise_Event()
        {
            var customer = await CreateSampleCustomerAsync();

            customer.Should().NotBeNull();
            customer.IsDeleted.Should().BeFalse();
            customer.DomainEvents.Should().ContainSingle(e => e is CustomerCreatedDomainEvent);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Customer_And_Raise_Event()
        {
            var customer = await CreateSampleCustomerAsync();

            await customer.UpdateAsync(
                FirstName.Create("Reza"),
                LastName.Create("Ahmadi"),
                DateOfBirth.Create(DateOnly.FromDateTime(DateTime.Today.AddYears(-30))),
                PhoneNumber.Create("+989121111111"),
                Email.Create("reza@test.com"),
                BankAccountNumber.Create("9876543210"),
                _uniquenessChecker.Object);

            customer.FirstName.Value.Should().Be("Reza");
            customer.LastName.Value.Should().Be("Ahmadi");
            customer.PhoneNumber.Number.Should().Be("9121111111");
            customer.DomainEvents.Should().ContainSingle(e => e is CustomerUpdatedDomainEvent);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Customer_Is_Deleted()
        {
            var customer = await CreateSampleCustomerAsync();
            customer.Delete();

            Func<Task> act = async () => await customer.UpdateAsync(
                FirstName.Create("Reza"),
                LastName.Create("Ahmadi"),
                DateOfBirth.Create(DateOnly.FromDateTime(DateTime.Today.AddYears(-30))),
                PhoneNumber.Create("+989121111111"),
                Email.Create("reza@test.com"),
                BankAccountNumber.Create("9876543210"),
                _uniquenessChecker.Object);

            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage("Deleted customer cannot be updated.");
        }

        [Fact]
        public async Task Delete_Should_Set_IsDeleted_And_Raise_Event()
        {
            var customer = await CreateSampleCustomerAsync();

            customer.Delete();

            customer.IsDeleted.Should().BeTrue();
            customer.DomainEvents.Should().ContainSingle(e => e is CustomerDeletedDomainEvent);
        }

        [Fact]
        public async Task Restore_Should_Set_IsDeleted_False_And_Raise_Event()
        {
            var customer = await CreateSampleCustomerAsync();
            customer.Delete();

            customer.Restore();

            customer.IsDeleted.Should().BeFalse();
            customer.DomainEvents.Should().ContainSingle(e => e is CustomerRestoredDomainEvent);
        }
    }
}