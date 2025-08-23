using Domain.Aggregates.Customer.Events;
using Domain.Aggregates.Customer.Rules;
using Domain.Aggregates.Customer.Services;
using Domain.Aggregates.Customer.ValueObjects;
using Domain.SeedWork;
using Domain.SeedWork.Rules;

namespace Domain.Aggregates.Customer
{
        public sealed class Customer : AggregateRoot<Guid>//, ISoftDelete
        {
            private FirstName _firstName;
            private LastName _lastName;
            private DateOfBirth _dateOfBirth;
            private PhoneNumber _phoneNumber;
            private Email _email;
            private BankAccountNumber _bankAccountNumber;

            // Public Getters for EF Core
            public FirstName FirstName => _firstName;
            public LastName LastName => _lastName;
            public DateOfBirth DateOfBirth => _dateOfBirth;
            public Email Email => _email;
            public PhoneNumber PhoneNumber => _phoneNumber;
            public BankAccountNumber BankAccountNumber => _bankAccountNumber;

            public bool IsDeleted { get; private set; }

            private Customer() { } // For EF Core

            private Customer(
                FirstName firstName,
                LastName lastName,
                DateOfBirth dateOfBirth,
                PhoneNumber phoneNumber,
                Email email,
                BankAccountNumber bankAccountNumber)
            {
                Id = Guid.NewGuid();
                _firstName = firstName;
                _lastName = lastName;
                _dateOfBirth = dateOfBirth;
                _phoneNumber = phoneNumber;
                _email = email;
                _bankAccountNumber = bankAccountNumber;
                IsDeleted = false;

                AddDomainEvent(new CustomerCreatedDomainEvent(Id));
            }

            public static async Task<Customer> CreateAsync(FirstName firstName,
                LastName lastName,
                DateOfBirth dateOfBirth,
                PhoneNumber phoneNumber,
                Email email,
                BankAccountNumber bankAccountNumber,
                ICustomerUniquenessCheckerService uniquenessChecker)
            {

                await RuleChecker.CheckRule(new CustomerEmailMustBeUniqueRule(email.Value, uniquenessChecker));
                await RuleChecker.CheckRule(new CustomerPersonalInfoMustBeUniqueRule(firstName.Value, lastName.Value, dateOfBirth.Value, uniquenessChecker));

                return new Customer(firstName, lastName, dateOfBirth, phoneNumber, email, bankAccountNumber);
            }

            public async Task UpdateAsync(
                FirstName firstName,
                LastName lastName,
                DateOfBirth dateOfBirth,
                PhoneNumber phoneNumber,
                Email email,
                BankAccountNumber bankAccountNumber,
                ICustomerUniquenessCheckerService uniquenessChecker)
            {
                if (IsDeleted)
                    throw new InvalidOperationException("Deleted customer cannot be updated.");

                await RuleChecker.CheckRule(new CustomerEmailMustBeUniqueRule(email.Value, uniquenessChecker));
                await RuleChecker.CheckRule(new CustomerPersonalInfoMustBeUniqueRule(firstName.Value, lastName.Value, dateOfBirth.Value, uniquenessChecker));


                _firstName = firstName;
                _lastName = lastName;
                _dateOfBirth = dateOfBirth;
                _phoneNumber = phoneNumber;
                _email = email;
                _bankAccountNumber = bankAccountNumber;

                AddDomainEvent(new CustomerUpdatedDomainEvent(Id));
            }

            public void Delete()
            {
                if (IsDeleted)
                    return;

                IsDeleted = true;
                AddDomainEvent(new CustomerDeletedDomainEvent(Id));
            }

            public void Restore()
            {
                if (!IsDeleted)
                    return;

                IsDeleted = false;
                AddDomainEvent(new CustomerRestoredDomainEvent(Id));
            }
        }
}
