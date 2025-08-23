using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.Repositories;
using Domain.Aggregates.Customer.Services;
using Domain.Aggregates.Customer.ValueObjects;
using MediatR;

namespace Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerUniquenessCheckerService _uniquenessChecker;

        public CreateCustomerCommandHandler(
            ICustomerRepository customerRepository,
            ICustomerUniquenessCheckerService uniquenessChecker)
        {
            _customerRepository = customerRepository;
            _uniquenessChecker = uniquenessChecker;
        }

        public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {

            var customer = await Customer.CreateAsync(
                 FirstName.Create(request.FirstName),
                 LastName.Create(request.LastName),
                 DateOfBirth.Create(request.DateOfBirth),
                 PhoneNumber.Create(request.PhoneNumber),
                 Email.Create(request.Email),
                 BankAccountNumber.Create(request.BankAccountNumber),
                 _uniquenessChecker
            );

            await _customerRepository.AddAsync(customer, cancellationToken);

            return customer.Id;
        }
    }
}
