using Application.Features.Customers.Dtos;
using AutoMapper;
using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.Repositories;
using MediatR;

namespace Application.Features.Customers.Queries.GetCustomersListQuery
{
    public class GetCustomersListQueryHandler : IRequestHandler<GetCustomersListQuery, List<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomersListQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<List<CustomerDto>> Handle(GetCustomersListQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository
                .GetAllAsync(request.Page, request.PageSize, cancellationToken);

            return _mapper.Map<List<CustomerDto>>(customers);
        }
    }

}
