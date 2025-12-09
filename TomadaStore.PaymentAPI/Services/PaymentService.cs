using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TomadaStore.Models.DTOs.Sale;

namespace TomadaStore.PaymentAPI.Services
{
    public class PaymentService
    {
        private readonly ConnectionFactory _factory;

        public PaymentService()
        {
            _factory = new ConnectionFactory { HostName = "localhost" };
        }

        public async Task MakePaymentAsync()
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

                var sale = new SalePaymentJsonDTO
                {
                    Customer = response.Customer,
                    Products = response.Products,
                    TotalPrice = response.TotalPrice,
                    PaymentApproval = (response.TotalPrice < 1000)
                };

                await ConfirmSaleAsync(sale);
            };

            await channel.BasicConsumeAsync("sale",
                                                autoAck: true,
                                                consumer: consumer);
        }

        public async Task ConfirmSaleAsync(SalePaymentJsonDTO sale)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "payment",
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

            string message = JsonSerializer.Serialize(sale);

            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty,
                                            routingKey: "payment",
                                            body: body);
        }
    }
}
