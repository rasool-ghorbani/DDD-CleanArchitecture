namespace Domain.Aggregates.Customer.Exception
{
    public class FirstNameMustNotBeEmptyException : ArgumentException
    {
        public FirstNameMustNotBeEmptyException() : base("First name must not be empty.")
        {
        }
    }
}
