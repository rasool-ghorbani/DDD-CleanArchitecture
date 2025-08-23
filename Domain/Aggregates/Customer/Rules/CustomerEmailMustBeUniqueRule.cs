using Domain.Aggregates.Customer.Services;
using Domain.SeedWork.Rules;

namespace Domain.Aggregates.Customer.Rules
{
    public class CustomerEmailMustBeUniqueRule : IBusinessRule
    {
        private readonly string _email;
        private readonly ICustomerUniquenessCheckerService _uniquenessChecker;

        public CustomerEmailMustBeUniqueRule(string email, ICustomerUniquenessCheckerService uniquenessChecker)
        {
            _email = email;
            _uniquenessChecker = uniquenessChecker;
        }

        public async Task<bool> IsBrokenAsync()
        {
            // چک می‌کنیم آیا ایمیل قبلا استفاده شده یا نه
            return !await _uniquenessChecker.IsEmailUniqueAsync(_email);
        }

        public string Message => "Customer email must be unique.";
    }
}
