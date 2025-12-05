using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.SaleAPI.Repositories.Interfaces;
using TomadaStore.SaleAPI.Services.Interfaces;

namespace TomadaStore.SaleAPI.Services
{
    public class SaleService : ISaleService
    {
        private readonly ILogger<SaleService> _logger;

        private readonly ISaleRepository _saleRepository;

        private readonly IHttpClientFactory _httpClientFactory;

        public SaleService(ILogger<SaleService> logger, ISaleRepository saleRepository, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _saleRepository = saleRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task CreateSaleAsync(int customerId, List<string> productIds)
        {
            try
            {
                var httpClientCustomer = _httpClientFactory.CreateClient("Customer");

                var customer = await httpClientCustomer
                                    .GetFromJsonAsync<CustomerResponseDTO>
                                    (customerId.ToString());


                var httpClientProduct = _httpClientFactory.CreateClient("Product");
                var products = new List<ProductResponseDTO>();

                foreach (var productId in productIds)
                {
                    var product = await httpClientProduct
                                        .GetFromJsonAsync<ProductResponseDTO>
                                        (productId);
                    products.Add(product);
                }

                if ((customer is null) || (products.Any(p => p == null)))
                    throw new Exception("Nonexistent Id was referenced.");

                await _saleRepository.CreateSaleAsync(customer, products);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error creating sale: {e.Message}");
                throw;
            }
        }
    }
}
