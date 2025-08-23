using Application.Exceptions;
using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.Repositories;
using MediatR;

namespace Application.Features.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly ICustomerRepository _repository;
        public DeleteCustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync(request.CustomerId, cancellationToken);

            if (customer is null)
                throw new NotFoundException(nameof(Customer), request.CustomerId);

            customer.Delete();

            await _repository.UpdateAsync(customer, cancellationToken);
        }
    }
}
