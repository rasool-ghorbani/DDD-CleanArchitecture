using Domain.Aggregates.Customer.Exception;
using Domain.SeedWork;
using System.Text.RegularExpressions;

namespace Domain.Aggregates.Customer.ValueObjects
{
    public sealed class PhoneNumber : ValueObject
    {
        private static readonly Regex CountryCodeRegex = new(@"^\+\d{1,4}$", RegexOptions.Compiled);
        private static readonly Regex NumberRegex = new(@"^\d{6,15}$", RegexOptions.Compiled);

        public string CountryCode { get; }
        public string Number { get; }

        private PhoneNumber(string countryCode, string number)
        {
            CountryCode = countryCode;
            Number = number;
        }

        public static PhoneNumber Create(string rawPhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(rawPhoneNumber))
                throw new PhoneNumberNameMustNotBeEmptyException();

            // فرض: ورودی شبیه "+989121234567" باشه
            if (!rawPhoneNumber.StartsWith("+"))
                throw new PhoneNumberStartWithPlusException();

            var match = Regex.Match(rawPhoneNumber, @"^\+(\d{1,2})(\d{9,10})$");

            if (!match.Success)
                throw new PhoneNumberInvalidFormatException();

            var countryCode = match.Groups[1].Value;
            var number = match.Groups[2].Value;

            return new PhoneNumber("+" + countryCode, number);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CountryCode;
            yield return Number;
        }

        public override string ToString() => $"{CountryCode}{Number}";
    }
}
