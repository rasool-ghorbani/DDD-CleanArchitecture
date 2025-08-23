using Domain.Aggregates.Customer.Exception;
using Domain.SeedWork;

namespace Domain.Aggregates.Customer.ValueObjects
{
    public sealed class LastName : ValueObject
    {
        public string Value { get; }

        private LastName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new LastNameMustNotBeEmptyException();

            value = value.Trim();

            if (value.Length < 2 || value.Length > 50)
                throw new LastNameLengthException();

            Value = value;
        }

        public static LastName Create(string value) => new LastName(value);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value.ToLowerInvariant();
        }

        public override string ToString() => Value;
    }
}
