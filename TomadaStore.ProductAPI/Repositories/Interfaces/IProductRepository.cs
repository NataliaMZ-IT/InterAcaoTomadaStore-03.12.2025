using TomadaStore.Models.DTOs.Product;

namespace TomadaStore.ProductAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductResponseDTO>> GetAllProductsAsync();
        Task<ProductResponseDTO?> GetProductByIdAsync(string id);
        Task CreateProductAsync(ProductRequestDTO product);
        Task UpdateProductAsync(string id, ProductRequestDTO product);
        Task DeleteProductAsync(string id);
    }
}
