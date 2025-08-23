namespace Domain.Aggregates.Customer.Exception
{
    public class BankAccountNumberOnlyDigitsException : ArgumentException
    {
        public BankAccountNumberOnlyDigitsException() : base("Bank account number must be between 10 and 26 digits.")
        {
        }
    }
}
