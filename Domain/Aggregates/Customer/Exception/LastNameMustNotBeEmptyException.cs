namespace Domain.Aggregates.Customer.Exception
{
    public class LastNameMustNotBeEmptyException : ArgumentException
    {
        public LastNameMustNotBeEmptyException() : base("Last name must not be empty.")
        {
        }
    }
}
