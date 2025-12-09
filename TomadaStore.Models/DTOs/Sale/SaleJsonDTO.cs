using MongoDB.Bson;
using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;

namespace TomadaStore.Models.DTOs.Sale
{
    public class SaleJsonDTO
    {
        public CustomerResponseDTO Customer { get; init; }
        public List<ProductResponseDTO> Products { get; init; }
        public DateTime SaleDate {  get; init; } = DateTime.UtcNow;
        public decimal TotalPrice { get; init; }
    }
}
