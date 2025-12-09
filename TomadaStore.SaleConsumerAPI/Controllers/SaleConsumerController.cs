using Microsoft.AspNetCore.Mvc;
using TomadaStore.SaleConsumerAPI.Services;

namespace TomadaStore.SaleConsumerAPI.Controllers
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class SaleConsumerController : ControllerBase
    {
        private readonly ILogger<SaleConsumerController> _logger;

        private readonly SaleConsumerService _saleConsumerService;

        public SaleConsumerController(ILogger<SaleConsumerController> logger, SaleConsumerService saleConsumerService)
        {
            _logger = logger;
            _saleConsumerService = saleConsumerService;
        }

        [HttpPost]
        public async Task<IActionResult> StoreSaleAsync()
        {
            try
            {
                await _saleConsumerService.StoreSaleAsync();

                return Created();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while storing sale");
                return Problem(e.Message);
            }
        }
    }
}
