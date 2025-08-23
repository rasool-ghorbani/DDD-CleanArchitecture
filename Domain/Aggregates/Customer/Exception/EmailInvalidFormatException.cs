using Domain.Aggregates.Customer.ValueObjects;

namespace Domain.Aggregates.Customer.Exception
{
    public class EmailInvalidFormatException : ArgumentException
    {
        public EmailInvalidFormatException() : base("Invalid email format.")
        {
        }
    }
}
