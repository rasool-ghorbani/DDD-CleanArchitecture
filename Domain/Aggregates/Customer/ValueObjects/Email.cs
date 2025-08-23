using Domain.Aggregates.Customer.Exception;
using Domain.SeedWork;
using System.Text.RegularExpressions;

namespace Domain.Aggregates.Customer.ValueObjects
{
    public sealed class Email : ValueObject
    {
        private static readonly Regex _emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public string Value { get; }

        private Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EmailMustNotBeEmptyException();

            if (!_emailRegex.IsMatch(value))
                throw new EmailInvalidFormatException();

            Value = value;
        }

        public static Email Create(string value) => new Email(value);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value.ToLowerInvariant();
        }

        public override string ToString() => Value;
    }
}
