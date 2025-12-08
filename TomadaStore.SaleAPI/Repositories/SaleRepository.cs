using MongoDB.Driver;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.Models;
using TomadaStore.SaleAPI.Data;
using TomadaStore.SaleAPI.Repositories.Interfaces;

namespace TomadaStore.SaleAPI.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ILogger<SaleRepository> _logger;
        private readonly IMongoCollection<Sale> _saleCollection;

        public SaleRepository(ILogger<SaleRepository> logger, ConnectionDB connection)
        {
            _logger = logger;
            _saleCollection = connection.GetCollection();
        }

        public async Task CreateSaleAsync(CustomerResponseDTO customer, 
                                          List<ProductResponseDTO> productList)
        {
            try
            {
                var customerSale = new Customer
                (
                    customer.Id,
                    customer.FirstName,
                    customer.LastName,
                    customer.Email,
                    customer.PhoneNumber,
                    customer.IsActive
                );

                var products= new List<Product>();
                decimal total = 0;

                foreach (var product in productList)
                {
                    var productSale = new Product
                    (
                        product.Id,
                        product.Name,
                        product.Description,
                        product.Price,
                        new Category
                        (
                            product.Category.Id,
                            product.Category.Name,
                            product.Category.Description
                        )
                    );
                    products.Add(productSale);
                    total += product.Price;
                }

                await _saleCollection.InsertOneAsync(new Sale
                (
                    customerSale,
                    products,
                    total
                ));
            }
            catch (Exception e)
            {
                _logger.LogError($"Error creating sale: {e.Message}");
                throw;
            }
        }
    }
}
