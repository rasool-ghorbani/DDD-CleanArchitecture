using Application.Features.Customers.Dtos;
using MediatR;

namespace Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery : IRequest<CustomerDto>
    {
        public Guid CustomerId { get; set; }
    }
}
