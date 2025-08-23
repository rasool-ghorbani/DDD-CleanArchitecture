namespace Domain.Aggregates.Customer.Exception
{
    public class BankAccountNumberNotBeEmptyException : ArgumentException
    {
        public BankAccountNumberNotBeEmptyException() : base("Bank account number must not be empty.")
        {
        }
    }
}
