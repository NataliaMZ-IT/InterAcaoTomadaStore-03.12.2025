using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TomadaStore.PaymentAPI.Services;

namespace TomadaStore.PaymentAPI.Controllers
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;

        private readonly PaymentService _paymentService;

        public PaymentController(ILogger<PaymentController> logger, PaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> MakePaymentAsync()
        {
            try
            {
                await _paymentService.MakePaymentAsync();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while making payment.");
                return Problem(e.Message);
            }
        }
    }
}
