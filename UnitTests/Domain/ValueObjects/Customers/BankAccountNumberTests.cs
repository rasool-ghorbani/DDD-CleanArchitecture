using Domain.Aggregates.Customer.Exception;
using Domain.Aggregates.Customer.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.ValueObjects.Customers
{
    public class BankAccountNumberTests
    {
        [Theory]
        [InlineData("1234567890")]
        [InlineData("12345678901234567890123456")] // 26 digits
        public void Create_Should_Return_Valid_BankAccountNumber_When_Value_Is_Valid(string value)
        {
            var account = BankAccountNumber.Create(value);

            account.Value.Should().Be(value);
            account.ToString().Should().Be(value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_Should_Throw_When_Value_Is_Empty(string value)
        {
            Action act = () => BankAccountNumber.Create(value);

            act.Should().Throw<BankAccountNumberNotBeEmptyException>();
        }

        [Theory]
        [InlineData("123456789")]      // < 10
        [InlineData("123456789012345678901234567")] // > 26
        public void Create_Should_Throw_When_Length_Is_Invalid(string value)
        {
            Action act = () => BankAccountNumber.Create(value);

            act.Should().Throw<BankAccountNumberLengthException>();
        }

        [Theory]
        [InlineData("12345abc678")]
        [InlineData("1234-567890")]
        public void Create_Should_Throw_When_Contains_NonDigit_Characters(string value)
        {
            Action act = () => BankAccountNumber.Create(value);

            act.Should().Throw<BankAccountNumberOnlyDigitsException>();
        }

        [Fact]
        public void Equality_Should_Be_Based_On_Value()
        {
            var account1 = BankAccountNumber.Create("1234567890");
            var account2 = BankAccountNumber.Create("1234567890");

            account1.Should().Be(account2);
        }
    }
}
