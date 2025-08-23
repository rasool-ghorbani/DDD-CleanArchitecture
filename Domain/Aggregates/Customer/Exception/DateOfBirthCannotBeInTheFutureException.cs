namespace Domain.Aggregates.Customer.Exception
{
    public class DateOfBirthCannotBeInTheFutureException : ArgumentException
    {
        public DateOfBirthCannotBeInTheFutureException() : base("Date of birth cannot be in the future.")
        {
        }
    }
}
