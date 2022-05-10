using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nxt.Entities.Dtos.Customer;
using Nxt.Services.Interfaces;
using System.Threading.Tasks;

namespace Nxt.API.Controllers
{
    //[Authorize(Roles = Roles.Admin)]
    [Authorize]
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _customerService.GetCustomers();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _customerService.GetCustomer(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerInput input)
        {
            var result = await _customerService.CreateCustomer(input);
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] CustomerInput input)
        {
            var result = await _customerService.UpdateCustomer(id, input);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customerService.DeleteCustomer(id);
            return Ok(result);
        }
    }
}
