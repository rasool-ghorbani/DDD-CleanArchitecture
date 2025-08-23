using MediatR;

namespace Application.Features.Customers.Commands.CreateCustomer
{
    public record CreateCustomerCommand(
     string FirstName,
     string LastName,
     DateOnly DateOfBirth,
     string PhoneNumber,
     string Email,
     string BankAccountNumber
        ) : IRequest<Guid>; // return created Customer Id
}
