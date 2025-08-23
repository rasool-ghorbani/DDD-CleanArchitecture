namespace Domain.Aggregates.Customer.Exception
{
    public class PhoneNumberNameMustNotBeEmptyException : ArgumentException
    {
        public PhoneNumberNameMustNotBeEmptyException() : base("Phone number name must not be empty.")
        {
        }
    }
}
