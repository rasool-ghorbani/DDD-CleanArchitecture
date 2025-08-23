using Application.Exceptions;
using Application.Features.Customers.Dtos;
using AutoMapper;
using Domain.Aggregates.Customer.Repositories;
using MediatR;

namespace Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);

            if (customer == null)
                throw new NotFoundException("Customer", request.CustomerId);

            return _mapper.Map<CustomerDto>(customer);
        }
    }
}
