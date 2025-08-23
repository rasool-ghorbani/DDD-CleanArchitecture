using Domain.Aggregates.Customer.Exception;
using Domain.Aggregates.Customer.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.Aggregates.Customer
{
    public class FirstNameTests
    {
        [Theory]
        [InlineData("Ali")]
        [InlineData(" Reza ")]
        public void Create_Should_Return_Valid_FirstName_When_Value_Is_Valid(string value)
        {
            var firstName = FirstName.Create(value);

            firstName.Value.Should().Be(value.Trim());
            firstName.ToString().Should().Be(value.Trim());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_Should_Throw_Exception_When_Value_Is_Empty(string value)
        {
            Action act = () => FirstName.Create(value);

            act.Should().Throw<FirstNameMustNotBeEmptyException>();
        }

        [Theory]
        [InlineData("A")]
        public void Create_Should_Throw_Exception_When_Length_Is_Invalid(string value)
        {
            Action act = () => FirstName.Create(value);

            act.Should().Throw<FirstNameLengthException>();
        }

        [Fact]
        public void FirstName_Should_Throw_When_TooLong()
        {
            var value = new string('a', 51);

            Action act = () => FirstName.Create(value);

            act.Should().Throw<FirstNameLengthException>();
        }

        [Fact]
        public void Equality_Should_Be_Case_Insensitive()
        {
            var name1 = FirstName.Create("Ali");
            var name2 = FirstName.Create("ali");

            name1.Should().Be(name2);
        }
    }
}
