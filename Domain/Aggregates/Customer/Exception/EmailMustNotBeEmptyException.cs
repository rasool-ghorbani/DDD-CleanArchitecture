namespace Domain.Aggregates.Customer.Exception
{
    public class EmailMustNotBeEmptyException : ArgumentException
    {
        public EmailMustNotBeEmptyException() : base("Email must not be empty.")
        {
        }
    }
}
