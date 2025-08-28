using Domain.Aggregates.Customer.Exception;
using Domain.Aggregates.Customer.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.ValueObjects.Customers
{
    public class DateOfBirthTests
    {
        [Fact]
        public void Create_Should_Return_Valid_DateOfBirth_When_Value_Is_Valid()
        {
            var value = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));

            var dob = DateOfBirth.Create(value);

            dob.Value.Should().Be(value);
            Assert.True(dob.GetAge() >= 18);
            dob.ToString().Should().Be(value.ToString("yyyy-MM-dd"));
        }

        [Fact]
        public void Create_Should_Throw_When_Date_Is_In_Future()
        {
            var futureDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

            Action act = () => DateOfBirth.Create(futureDate);

            act.Should().Throw<DateOfBirthCannotBeInTheFutureException>();
        }

        [Fact]
        public void Create_Should_Throw_When_Age_Is_Less_Than_18()
        {
            var under18 = DateOnly.FromDateTime(DateTime.Today.AddYears(-17));

            Action act = () => DateOfBirth.Create(under18);

            act.Should().Throw<DateOfBirthLeast18YearsException>();
        }

        [Fact]
        public void Equality_Should_Work_Based_On_Date_Value()
        {
            var date = DateOnly.FromDateTime(DateTime.Today.AddYears(-25));

            var dob1 = DateOfBirth.Create(date);
            var dob2 = DateOfBirth.Create(date);

            dob1.Should().Be(dob2);
        }
    }
}
