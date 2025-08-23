using Application.Features.Customers.Dtos;
using MediatR;

namespace Application.Features.Customers.Queries.GetCustomersListQuery
{
    public class GetCustomersListQuery : IRequest<List<CustomerDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
