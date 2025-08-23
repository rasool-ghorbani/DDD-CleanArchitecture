using Domain.Aggregates.Customer.Services;
using Domain.SeedWork.Rules;

namespace Domain.Aggregates.Customer.Rules
{
    public class CustomerPersonalInfoMustBeUniqueRule : IBusinessRule
    {
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly DateOnly _dateOfBirth;
        private readonly ICustomerUniquenessCheckerService _uniquenessChecker;

        public CustomerPersonalInfoMustBeUniqueRule(string firstName, string lastName, DateOnly dateOfBirth, ICustomerUniquenessCheckerService uniquenessChecker)
        {
            _firstName = firstName;
            _lastName = lastName;
            _dateOfBirth = dateOfBirth;
            _uniquenessChecker = uniquenessChecker;
        }

        public async Task<bool> IsBrokenAsync()
        {
            return !await _uniquenessChecker.IsPersonalInfoUniqueAsync(_firstName, _lastName, _dateOfBirth);
        }

        public string Message => "Customer personal information must be unique.";
    }
}
