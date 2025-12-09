using TomadaStore.Models.DTOs.Customer;
using TomadaStore.Models.DTOs.Product;

namespace TomadaStore.Models.DTOs.Sale
{
    public class SalePaymentJsonDTO
    {
        public CustomerResponseDTO Customer { get; init; }
        public List<ProductResponseDTO> Products { get; init; }
        public decimal TotalPrice { get; init; }
        public bool PaymentApproval { get; init; }
    }
}
