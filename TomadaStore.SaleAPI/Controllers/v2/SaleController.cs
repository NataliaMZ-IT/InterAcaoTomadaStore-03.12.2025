using Microsoft.AspNetCore.Mvc;
using TomadaStore.SaleAPI.Services.v2;

namespace TomadaStore.SaleAPI.Controllers.v2
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ILogger<SaleController> _logger;
        private readonly SaleServiceV2 _saleService;

        public SaleController(ILogger<SaleController> logger, SaleServiceV2 saleService)
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
