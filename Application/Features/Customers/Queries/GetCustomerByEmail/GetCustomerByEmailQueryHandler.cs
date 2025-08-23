using Application.Exceptions;
using Application.Features.Customers.Dtos;
using AutoMapper;
using Domain.Aggregates.Customer.Repositories;
using MediatR;

namespace Application.Features.Customers.Queries.GetCustomerByEmail
{
    public class GetCustomerByEmailQueryHandler : IRequestHandler<GetCustomerByEmailQuery, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetCustomerByEmailQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (customer == null)
                throw new NotFoundException($"Customer with email '{request.Email}' was not found.", request.Email);

            return _mapper.Map<CustomerDto>(customer);
        }
    }
}
