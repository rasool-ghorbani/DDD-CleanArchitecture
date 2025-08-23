namespace Domain.Aggregates.Customer.Exception
{
    public class LastNameLengthException : ArgumentException
    {
        public LastNameLengthException() : base("Last name must be between 2 and 50 characters.")
        {
        }
    }
}
