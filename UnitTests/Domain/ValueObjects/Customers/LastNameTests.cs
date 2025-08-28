using Domain.Aggregates.Customer.Exception;
using Domain.Aggregates.Customer.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.ValueObjects.Customers
{
    public class LastNameTests
    {
        [Theory]
        [InlineData("Hosseini")]
        [InlineData("   Karimi   ")]
        public void Create_Should_Return_Valid_LastName_When_Value_Is_Valid(string value)
        {
            var lastName = LastName.Create(value);

            lastName.Value.Should().Be(value.Trim());
            lastName.ToString().Should().Be(value.Trim());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_Should_Throw_Exception_When_Value_Is_Empty(string value)
        {
            Action act = () => LastName.Create(value);

            act.Should().Throw<LastNameMustNotBeEmptyException>();
        }

        [Theory]
        [InlineData("A")]
        public void Create_Should_Throw_Exception_When_Length_Is_Invalid(string value)
        {
            Action act = () => LastName.Create(value);

            act.Should().Throw<LastNameLengthException>();
        }

        [Fact]
        public void LastName_Should_Throw_When_TooLong()
        {
            var value = new string('a', 51);

            Action act = () => LastName.Create(value);

            act.Should().Throw<LastNameLengthException>();
        }

        [Fact]
        public void Equality_Should_Be_Case_Insensitive()
        {
            var name1 = LastName.Create("Hosseini");
            var name2 = LastName.Create("hosseini");

            name1.Should().Be(name2);
        }
    }
}
