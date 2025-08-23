namespace Domain.Aggregates.Customer.Exception
{
    public class BankAccountNumberLengthException : ArgumentException
    {
        public BankAccountNumberLengthException() : base("Bank account number must be between 10 and 26 digits.")
        {
        }
    }
}
