using MediatR;

namespace Application.Features.Customers.Commands.DeleteCustomer
{
    public record DeleteCustomerCommand(Guid CustomerId) : IRequest;
}
