using Domain.Aggregates.Customer.Exception;
using Domain.Aggregates.Customer.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.ValueObjects.Customers
{
    public class EmailTests
    {
        [Theory]
        [InlineData("test@example.com")]
        [InlineData("user.name@domain.co")]
        [InlineData("USER@DOMAIN.COM")]
        public void Create_Should_Return_Valid_Email_When_Value_Is_Valid(string value)
        {
            var email = Email.Create(value);

            email.Value.Should().Be(value);
            email.ToString().Should().Be(value);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_Should_Throw_Exception_When_Value_Is_Empty(string value)
        {
            Action act = () => Email.Create(value);

            act.Should().Throw<EmailMustNotBeEmptyException>();
        }

        [Theory]
        [InlineData("plainaddress")]
        [InlineData("missing-at-sign.com")]
        [InlineData("user@")]
        [InlineData("@domain.com")]
        [InlineData("user@domain")]
        public void Create_Should_Throw_Exception_When_Value_Is_Invalid_Format(string value)
        {
            Action act = () => Email.Create(value);

            act.Should().Throw<EmailInvalidFormatException>();
        }

        [Fact]
        public void Equality_Should_Be_Case_Insensitive()
        {
            var email1 = Email.Create("Test@Example.com");
            var email2 = Email.Create("test@example.com");

            email1.Should().Be(email2);
        }
    }
}
