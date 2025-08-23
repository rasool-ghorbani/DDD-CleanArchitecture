using Domain.Aggregates.Customer.Exception;
using Domain.SeedWork;

namespace Domain.Aggregates.Customer.ValueObjects
{
    public sealed class DateOfBirth : ValueObject
    {
        public DateOnly Value { get; }

        private DateOfBirth(DateOnly value)
        {
            if (value > DateOnly.FromDateTime(DateTime.Today))
                throw new DateOfBirthCannotBeInTheFutureException();

            var age = GetAge(value);

            if (age < 18)
                throw new DateOfBirthLeast18YearsException();

            Value = value;
        }

        public static DateOfBirth Create(DateOnly value) => new DateOfBirth(value);

        public int GetAge() => GetAge(Value);

        private static int GetAge(DateOnly dob)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;
            return age;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value.ToString("yyyy-MM-dd");
    }
}
