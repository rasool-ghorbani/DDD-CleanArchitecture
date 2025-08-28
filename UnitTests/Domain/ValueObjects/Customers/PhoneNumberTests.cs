using Domain.Aggregates.Customer.Exception;
using Domain.Aggregates.Customer.ValueObjects;
using FluentAssertions;

namespace UnitTests.Domain.ValueObjects.Customers
{
    public class PhoneNumberTests
    {
        [Theory]
        [InlineData("+989121234567", "+98", "9121234567")]
        [InlineData("+011234567890", "+01", "1234567890")]
        public void Create_Should_Parse_Valid_RawPhoneNumber(string raw, string expectedCode, string expectedNumber)
        {
            var phone = PhoneNumber.Create(raw);

            phone.CountryCode.Should().Be(expectedCode);
            phone.Number.Should().Be(expectedNumber);
            phone.ToString().Should().Be(expectedCode + expectedNumber);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_Should_Throw_When_Empty(string raw)
        {
            Action act = () => PhoneNumber.Create(raw);

            act.Should().Throw<PhoneNumberNameMustNotBeEmptyException>();
        }

        [Theory]
        [InlineData("989121234567")]  // بدون +
        [InlineData("09121234567")]   // بدون +
        public void Create_Should_Throw_When_Not_StartWithPlus(string raw)
        {
            Action act = () => PhoneNumber.Create(raw);

            act.Should().Throw<PhoneNumberStartWithPlusException>();
        }

        [Theory]
        [InlineData("+981234")]            // کمتر از حد مجاز
        [InlineData("+98123456789012345")] // بیشتر از حد مجاز
        [InlineData("+98abcdefg123")]      // حروف غیر مجاز
        public void Create_Should_Throw_When_Invalid_Format(string raw)
        {
            Action act = () => PhoneNumber.Create(raw);

            act.Should().Throw<PhoneNumberInvalidFormatException>();
        }

        [Fact]
        public void Equality_Should_Be_Based_On_CountryCode_And_Number()
        {
            var phone1 = PhoneNumber.Create("+989121234567");
            var phone2 = PhoneNumber.Create("+989121234567");

            phone1.Should().Be(phone2);
        }
    }
}
