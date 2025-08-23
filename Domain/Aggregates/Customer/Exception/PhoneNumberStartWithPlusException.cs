namespace Domain.Aggregates.Customer.Exception
{
    public class PhoneNumberStartWithPlusException : ArgumentException
    {
        public PhoneNumberStartWithPlusException() : base("Phone number must start with '+'.")
        {
        }
    }
}
