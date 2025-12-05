using Microsoft.AspNetCore.Mvc;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.SaleAPI.Services.Interfaces;

namespace TomadaStore.SaleAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ILogger<SaleController> _logger;
        private readonly ISaleService _saleService;

        public SaleController(ILogger<SaleController> logger, ISaleService saleService)
        {
            _logger = logger;
            _saleService = saleService;
        }

        [HttpPost("customer/{customerId}")]
        public async Task<IActionResult> CreateSaleAsync(int customerId,
                                                         [FromBody] List<string> productIds)
        {
            try
            {
                await _saleService.CreateSaleAsync(customerId, productIds);

                return Created();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while creating sale.");
                return Problem(e.Message);
            }
        }
    }
}
