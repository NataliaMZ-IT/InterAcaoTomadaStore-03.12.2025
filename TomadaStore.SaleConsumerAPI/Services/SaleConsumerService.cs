using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TomadaStore.Models.DTOs.Sale;
using TomadaStore.Models.Models;
using TomadaStore.SaleConsumerAPI.Repositories;

namespace TomadaStore.SaleConsumerAPI.Services
{
    public class SaleConsumerService
    {
        private readonly ILogger<SaleConsumerService> _logger;

        private readonly SaleRepository _saleRepository;

        private readonly ConnectionFactory _factory;

        public SaleConsumerService(ILogger<SaleConsumerService> logger, SaleRepository saleRepository)
        {
            _logger = logger;
            _saleRepository = saleRepository;
            _factory = new ConnectionFactory { HostName = "localhost" };
        }

        public async Task StoreSaleAsync()
        {
            try
            {
                using var connection = await _factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(queue: "sale",
                                                durable: false,
                                                exclusive: false,
                                                autoDelete: false,
                                                arguments: null);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var response = JsonSerializer.Deserialize<SaleJsonDTO>(message);

                    await _saleRepository.StoreSaleAsync(DTOtoSale(response));
                };

                await channel.BasicConsumeAsync("sale",
                                                autoAck: true,
                                                consumer: consumer);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error storing sale in database.");
                throw;
            }
        }

        public Sale DTOtoSale(SaleJsonDTO dto)
        {
            Customer customer = new(dto.Customer.Id,
                                    dto.Customer.FirstName,
                                    dto.Customer.LastName,
                                    dto.Customer.Email,
                                    dto.Customer.PhoneNumber,
                                    dto.Customer.IsActive
                                    );

            var products = new List<Product>();
            foreach(var dtoProduct in dto.Products)
            {
                Product product = new(dtoProduct.Id,
                                      dtoProduct.Name,
                                      dtoProduct.Description,
                                      dtoProduct.Price,
                                      new Category(
                                          dtoProduct.Category.Id,
                                          dtoProduct.Category.Name,
                                          dtoProduct.Category.Description)
                                      );
                products.Add(product);
            }
            return new Sale(customer, products, dto.TotalPrice);
        }
    }
}
