using Domain.Aggregates.Customer.Exception;
using Domain.SeedWork;

namespace Domain.Aggregates.Customer.ValueObjects
{
    public sealed class BankAccountNumber : ValueObject
    {
        public string Value { get; }

        private BankAccountNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new BankAccountNumberNotBeEmptyException();

            value = value.Trim();

            if (value.Length < 10 || value.Length > 26)
                throw new BankAccountNumberLengthException();

            if (!value.All(char.IsDigit))
                throw new BankAccountNumberOnlyDigitsException();

            Value = value;
        }

        public static BankAccountNumber Create(string value) => new BankAccountNumber(value);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}
