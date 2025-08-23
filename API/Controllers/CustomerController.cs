using Application.Features.Customers.Commands.CreateCustomer;
using Application.Features.Customers.Commands.DeleteCustomer;
using Application.Features.Customers.Commands.UpdateCustomer;
using Application.Features.Customers.Queries.GetCustomerByEmail;
using Application.Features.Customers.Queries.GetCustomerById;
using Application.Features.Customers.Queries.GetCustomersListQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class CustomerController : ControllerBase
    {
        private readonly ISender _mediatr;

        public CustomerController(ISender mediatr)
        {
            _mediatr = mediatr;
        }

        [HttpGet] 
        public async Task<IActionResult> Gets([FromQuery] GetCustomersListQuery payload, CancellationToken cancellationToken = default)
        {
            var result = await _mediatr.Send(payload, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] GetCustomerByIdQuery payload, CancellationToken cancellationToken = default)
        {
            var result = await _mediatr.Send(payload, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByEmail([FromQuery] GetCustomerByEmailQuery payload, CancellationToken cancellationToken = default)
        {
            var result = await _mediatr.Send(payload, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCustomerCommand payload, CancellationToken cancellationToken = default)
        {
            var result = await _mediatr.Send(payload, cancellationToken);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerCommand payload, CancellationToken cancellationToken = default)
        {
            await _mediatr.Send(payload, cancellationToken);
            return Ok(null);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteCustomerCommand payload, CancellationToken cancellationToken = default)
        {
            await _mediatr.Send(payload, cancellationToken);
            return Ok(null);
        }
    }
}
