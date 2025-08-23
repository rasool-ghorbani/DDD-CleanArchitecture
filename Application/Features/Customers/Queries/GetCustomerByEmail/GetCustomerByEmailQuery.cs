using Application.Features.Customers.Dtos;
using MediatR;

namespace Application.Features.Customers.Queries.GetCustomerByEmail
{
    public class GetCustomerByEmailQuery : IRequest<CustomerDto>
    {
        public string Email { get; set; }
    }
}
