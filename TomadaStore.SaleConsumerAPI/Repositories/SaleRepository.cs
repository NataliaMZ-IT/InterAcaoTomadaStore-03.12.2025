using MongoDB.Driver;
using TomadaStore.Models.Models;
using TomadaStore.SaleConsumerAPI.Data;

namespace TomadaStore.SaleConsumerAPI.Repositories
{
    public class SaleRepository
    {
        private readonly ILogger<SaleRepository> _logger;
        private readonly IMongoCollection<Sale> _saleCollection;

        public SaleRepository(ILogger<SaleRepository> logger, ConnectionDB connection)
        {
            _logger = logger;
            _saleCollection = connection.GetCollection();
        }

        public async Task StoreSaleAsync(Sale sale)
        {
            try
            {
                await _saleCollection.InsertOneAsync(sale);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error storing sale in database.");
                throw;
            }
        }
    }
}
