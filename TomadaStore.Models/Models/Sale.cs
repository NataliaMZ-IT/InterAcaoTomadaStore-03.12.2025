using MongoDB.Bson;

namespace TomadaStore.Models.Models
{
    public class Sale
    {
        public ObjectId Id { get; private set; }
        public Customer Customer { get; private set; }
        public List<Product> Products { get; private set; }
        public DateTime SaleDate { get; private set; }
        public decimal TotalPrice { get; private set; }
        public bool PaymentApproval { get; private set; }

        public Sale(Customer customer, List<Product> products, DateTime date, decimal totalPrice, bool approved)
        {
            Id = new ObjectId();
            Customer = customer;
            Products = products;
            SaleDate = date;
            TotalPrice = totalPrice;
            PaymentApproval = approved;
        }
    }
}
