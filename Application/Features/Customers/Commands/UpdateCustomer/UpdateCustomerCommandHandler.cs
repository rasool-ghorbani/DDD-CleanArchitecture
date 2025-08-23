using Application.Exceptions;
using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.Repositories;
using Domain.Aggregates.Customer.Services;
using Domain.Aggregates.Customer.ValueObjects;
using MediatR;

namespace Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
    {
        private readonly ICustomerRepository _repository;
        private readonly ICustomerUniquenessCheckerService _uniquenessChecker;

        public UpdateCustomerCommandHandler(ICustomerRepository repository,
            ICustomerUniquenessCheckerService uniquenessChecker)
        {
            _repository = repository;
            _uniquenessChecker = uniquenessChecker;
        }

        public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (customer is null)
                throw new NotFoundException(nameof(Customer), request.Id);

            await customer.UpdateAsync(
                  FirstName.Create(request.FirstName),
                  LastName.Create(request.LastName),
                  DateOfBirth.Create(request.DateOfBirth),
                  PhoneNumber.Create(request.PhoneNumber),
                  Email.Create(request.Email),
                  BankAccountNumber.Create(request.BankAccountNumber),
                  _uniquenessChecker
              );

            await _repository.UpdateAsync(customer, cancellationToken);
        }
    }
}
