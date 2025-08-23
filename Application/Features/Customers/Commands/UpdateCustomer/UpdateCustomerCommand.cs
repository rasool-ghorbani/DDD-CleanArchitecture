using MediatR;

namespace Application.Features.Customers.Commands.UpdateCustomer
{
    public record UpdateCustomerCommand(

         Guid Id,
         string FirstName,
         string LastName,
         DateOnly DateOfBirth,
         string PhoneNumber,
         string Email,
         string BankAccountNumber
        ) : IRequest;

}
