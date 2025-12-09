using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.Models;
using TomadaStore.SaleAPI.Services.Interfaces;

namespace TomadaStore.SaleAPI.Services.v2
{
    public class SaleServiceV2 : ISaleService
    {
        private readonly ILogger<SaleServiceV2> _logger;

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly ConnectionFactory _connectionFactory;

        public SaleServiceV2(ILogger<SaleServiceV2> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _connectionFactory = new ConnectionFactory { HostName = "localhost" };
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
                decimal totalPrice = 0;

                foreach (var productId in productIds)
                {
                    var product = await httpClientProduct
                                        .GetFromJsonAsync<ProductResponseDTO>
                                        (productId);
                    products.Add(product);
                    totalPrice += product.Price;
                }

                if ((customer is null) || (products.Any(p => p == null)))
                    throw new Exception("Nonexistent Id was referenced.");


                var sale = new SaleJsonDTO
                {
                    Customer = customer,
                    Products = products,
                    TotalPrice = totalPrice
                };

                await ProduceSale(sale);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error creating sale: {e.Message}");
                throw;
            }
        }

        public async Task ProduceSale(SaleJsonDTO sale)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "sale",
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

            string message = JsonSerializer.Serialize<SaleJsonDTO>(sale);

            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty,
                                            routingKey: "sale",
                                            body: body);
        }
    }
}
