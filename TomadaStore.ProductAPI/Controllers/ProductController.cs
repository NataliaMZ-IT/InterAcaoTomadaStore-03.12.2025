using Microsoft.AspNetCore.Mvc;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.ProductAPI.Services.Interfaces;

namespace TomadaStore.ProductAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();

                return Ok(products);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while getting all products.");
                return Problem(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateProductAsync([FromBody] ProductRequestDTO product)
        {
            try
            {
                await _productService.CreateProductAsync(product);

                return Created();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while creating product.");
                return Problem(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdAsync(string id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product is null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while getting product by id.");
                return Problem(e.Message);
            }
        }
    }
}
