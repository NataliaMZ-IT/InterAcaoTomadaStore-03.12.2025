using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Data.SqlTypes;
using TomadaStore.Models.DTOs.Category;
using TomadaStore.Models.DTOs.Product;
using TomadaStore.Models.Models;
using TomadaStore.ProductAPI.Data;
using TomadaStore.ProductAPI.Repositories.Interfaces;

namespace TomadaStore.ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        private readonly IMongoCollection<Product> _productCollection;

        public ProductRepository(ILogger<ProductRepository> logger, 
                                 ConnectionDB connection)
        {
            _logger = logger;
            _productCollection = connection.GetCollection();
        }

        public async Task CreateProductAsync(ProductRequestDTO product)
        {
            try
            {
                await _productCollection.InsertOneAsync(new Product
                    (
                        product.Name,
                        product.Description,
                        product.Price,
                        new Category
                        (
                            product.Category.Name,
                            product.Category.Description
                        )
                    ));
            }
            catch (Exception e)
            {
                _logger.LogError($"Error creating product: {e.Message}");
                throw;
            }
        }

        public Task DeleteProductAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductResponseDTO>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ProductResponseDTO?> GetProductByIdAsync(string id)
        {
            try
            {
                var product = await _productCollection.Find(p => p.Id == new ObjectId(id)).FirstOrDefaultAsync();

                ProductResponseDTO productResponse = new()
                {
                    Id = product.Id.ToString(),
                    Name = product.Name, 
                    Description = product.Description,
                    Price = product.Price, 
                    Category = new CategoryResponseDTO
                    {
                        Id = product.Category.Id.ToString(),
                        Name = product.Category.Name,
                        Description = product.Category.Description
                    }
                };

                return productResponse;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error getting product by id: {e.Message}");
                throw;
            }
        }

        public Task UpdateProductAsync(string id, ProductRequestDTO product)
        {
            throw new NotImplementedException();
        }
    }
}
