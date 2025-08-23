using Application.Features.Customers.Commands.CreateCustomer;
using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.Repositories;
using Domain.Aggregates.Customer.Rules;
using Domain.Aggregates.Customer.Services;
using Domain.SeedWork.Rules;
using FluentAssertions;
using Moq;

namespace UnitTests.Application.Features.Customers.Commands
{
    public class CreateCustomerCommandHandlerTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<ICustomerUniquenessCheckerService> _uniquenessCheckerMock;
        private readonly CreateCustomerCommandHandler _handler;

        public CreateCustomerCommandHandlerTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _uniquenessCheckerMock = new Mock<ICustomerUniquenessCheckerService>();

            _handler = new CreateCustomerCommandHandler(
                _customerRepositoryMock.Object,
                _uniquenessCheckerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldCreateCustomerAndReturnId()
        {
            // Arrange
            var request = new CreateCustomerCommand
            (
                "Ali",
                "Ahmadi",
                new DateOnly(1980, 01, 01),
                "+989186566156",
                "ali.ahmadi@example.com",
                "125478547854"
            );

            var expectedId = Guid.NewGuid(); // یک Guid فرضی برای تست

            // Mock کردن سرویس‌ها
            _uniquenessCheckerMock
                .Setup(x => x.IsEmailUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _uniquenessCheckerMock
               .Setup(x => x.IsPersonalInfoUniqueAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(true);

            // حالا Setup باید Task<Guid> برگرداند
            _customerRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedId); // متد ReturnsAsync برای Task<T> است

            // Act
            var customerId = await _handler.Handle(request, CancellationToken.None);

            // Assert
            customerId.Should().NotBeEmpty(); // بررسی می‌کنیم که Id برگشتی همان Id فرضی ما باشد
            _customerRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_DuplicateEmail_ShouldThrowException()
        {
            // Arrange
            var request = new CreateCustomerCommand
             ("Ali", "Ahmadi", new DateOnly(1980, 01, 01), "+989186566156", "ali.ahmadi@example.com", "125478547854");

            // Mock کردن سرویس UniquenessChecker به طوری که false برگرداند
            _uniquenessCheckerMock
                .Setup(x => x.IsEmailUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var rule = new CustomerEmailMustBeUniqueRule(request.Email, _uniquenessCheckerMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessRuleValidationException>(() => _handler.Handle(request, CancellationToken.None));

            // Assert
            exception.BrokenRule.Message.Should().Be(rule.Message);
        }

        [Fact]
        public async Task Handle_DuplicatePersonalInfo_ShouldThrowException()
        {
            // Arrange
            var request = new CreateCustomerCommand
             ("Ali", "Ahmadi", new DateOnly(1980, 01, 01), "+989186566156", "ali.ahmadi@example.com", "125478547854");

            // Mock کردن سرویس UniquenessChecker به طوری که false برگرداند
            _uniquenessCheckerMock
                .Setup(x => x.IsEmailUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _uniquenessCheckerMock
               .Setup(x => x.IsPersonalInfoUniqueAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(false);

            var rule = new CustomerPersonalInfoMustBeUniqueRule(request.FirstName, request.LastName, request.DateOfBirth, _uniquenessCheckerMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessRuleValidationException>(() => _handler.Handle(request, CancellationToken.None));

            // Assert
            exception.BrokenRule.Message.Should().Be(rule.Message);
        }
    }
}
