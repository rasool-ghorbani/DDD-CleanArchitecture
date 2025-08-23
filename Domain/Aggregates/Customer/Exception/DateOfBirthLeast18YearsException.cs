namespace Domain.Aggregates.Customer.Exception
{
    public class DateOfBirthLeast18YearsException : ArgumentException
    {
        public DateOfBirthLeast18YearsException() : base("Customer must be at least 18 years old.")
        {
        }
    }
}
