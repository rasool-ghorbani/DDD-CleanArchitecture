namespace Domain.Aggregates.Customer.Exception
{
    public class FirstNameLengthException : ArgumentException
    {
        public FirstNameLengthException() : base("First name must be between 2 and 50 characters.")
        {
        }
    }
}
