namespace Domain.Aggregates.Customer.Exception
{
    public class PhoneNumberInvalidFormatException : ArgumentException
    {
        public PhoneNumberInvalidFormatException() : base("Phone number format is invalid.")
        {
        }
    }
}
