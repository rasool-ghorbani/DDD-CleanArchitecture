using FluentValidation;

namespace Application.Features.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("CustomerId must be a valid non-empty GUID.");
        }
    }
}
