using Domain.Aggregates.Customer.Exception;
using Domain.SeedWork;

namespace Domain.Aggregates.Customer.ValueObjects
{
    public sealed class FirstName : ValueObject
    {
        public string Value { get; }

        private FirstName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new FirstNameMustNotBeEmptyException();

            value = value.Trim();

            if (value.Length < 2 || value.Length > 50)
                throw new FirstNameLengthException();

            Value = value;
        }

        public static FirstName Create(string value) => new FirstName(value);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value.ToLowerInvariant();
        }

        public override string ToString() => Value;
    }
}
