using Microsoft.AspNetCore.Mvc;
using TomadaStore.CustomerAPI.Services.Interfaces;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.Models;

namespace TomadaStore.CustomerAPI.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, 
                                    ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomerAsync([FromBody] CustomerRequestDTO customer)
        {
            try
            {
                _logger.LogInformation("Creating a new customer.");

                await _customerService.InsertCustomerAsync(customer);

                return Created();
            }
            catch (Exception e) 
            {
                _logger.LogError(e, "Error occurred while creating a new customer. " +
                    e.Message);

                return Problem(e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerResponseDTO>>> GetAllCustomersAsync()
        {
            try
            {
                var customers = await _customerService.GetAllCustomersAsync();

                return Ok(customers);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while getting customer list. " +
                    e.Message);

                return Problem(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponseDTO?>> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
                if (customer is null)
                    return NotFound();

                return Ok(customer);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while getting customer by id. " +
                    e.Message);
                return Problem(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> InactivateCustomerAsync(int id)
        {
            try
            {
                await _customerService.InactivateCustomerAsync(id);

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured while inactivating customer. " +
                    e.Message);
                return Problem(e.Message);
            }
        }
    }
}
