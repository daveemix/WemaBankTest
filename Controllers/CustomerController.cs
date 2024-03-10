using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WemaCustomer.Application.Features.Command;
using WemaCustomer.Application.Features.Commands;
using WemaCustomer.Application.Features.Queries;
using WemaCustomer.Helpers;

namespace WemaCustomer.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IMediator mediator, ILogger<CustomerController> logger)
        {
            _mediator = mediator;
            _logger = logger;

        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _mediator.Send(request);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(500, response); 
            }
        }



        [HttpPost("complete-onboarding")]
        public async Task<IActionResult> CompleteOnboarding([FromBody] CompleteOnboardingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _mediator.Send(request);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }



        [HttpGet("all")]
        public async Task<IActionResult> GetAllCustomers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var request = new GetAllCustomersRequest { PageNumber = pageNumber, PageSize = pageSize };
            var response = await _mediator.Send(request);

            return Ok(response);
        }



        [HttpGet("all-banks")]
        public async Task<IActionResult> GetAllBanks()
        {
            var response = await _mediator.Send(new GetAllBanksRequest());

            if (response.Success)
            {
                return Ok(response.Data);
            }
            else
            {
                _logger.LogError(response.Message);
                return StatusCode(500, response.Message);
            }
        }
    }
}
