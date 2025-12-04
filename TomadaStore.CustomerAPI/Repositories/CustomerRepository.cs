using Dapper;
using Microsoft.Data.SqlClient;
using TomadaStore.CustomerAPI.Data;
using TomadaStore.CustomerAPI.Repositories.Interfaces;
using TomadaStore.Models.DTOs.Customer;

namespace TomadaStore.CustomerAPI.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ILogger<CustomerRepository> _logger;
        private readonly SqlConnection _connection;
        public CustomerRepository(ILogger<CustomerRepository> logger, ConnectionDB connection)
        {
            _logger = logger;
            _connection = connection.GetConnection();
        }

        public async Task<List<CustomerResponseDTO>> GetAllCustomersAsync()
        {
            try
            {
                var sqlSelect = @"SELECT Id, FirstName, LastName, Email, PhoneNumber, IsActive
                                  FROM Customers";

                return (await _connection.QueryAsync<CustomerResponseDTO>(sqlSelect)).ToList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError($"SQL Error selecting customers: {sqlEx.Message}");
                throw new Exception(sqlEx.StackTrace);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error selecting customers: {e.Message}");
                throw new Exception(e.StackTrace);
            }
        }

        public async Task<CustomerResponseDTO?> GetCustomerByIdAsync(int id)
        {
            try
            {
                var sqlSelect = @"SELECT Id, FirstName, LastName, Email, PhoneNumber, IsActive
                                  FROM Customers
                                  WHERE Id = @Id";

                return await _connection.QueryFirstOrDefaultAsync<CustomerResponseDTO>(sqlSelect, new { Id = id });
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError($"SQL Error selecting customer by id: {sqlEx.Message}");
                throw new Exception(sqlEx.StackTrace);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error selecting customer by id: {e.Message}");
                throw new Exception(e.StackTrace);
            }
        }

        public async Task InsertCustomerAsync(CustomerRequestDTO customer)
        {
            try
            {
                var insertSql = @"INSERT INTO Customers (FirstName, LastName, Email, PhoneNumber, IsActive) 
                                  VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @IsActive)";

                await _connection.ExecuteAsync(insertSql, new { customer.FirstName,
                                                                customer.LastName,
                                                                customer.Email,
                                                                customer.PhoneNumber,
                                                                IsActive = true });
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError($"SQL Error inserting customer: {sqlEx.Message}");
                throw new Exception(sqlEx.StackTrace);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error inserting customer: {e.Message}");
                throw new Exception(e.StackTrace);
            }
        }

        public async Task ChangeCustomerStatusAsync(int id, bool status)
        {
            try
            {
                var deleteSql = @"UPDATE Customers SET IsActive = @IsActive WHERE iD = @Id";

                await _connection.ExecuteAsync(deleteSql, new { IsActive = status, Id = id });
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError($"SQL Error inactivating customer: {sqlEx.Message}");
                throw new Exception(sqlEx.StackTrace);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error inactivating customer: {e.Message}");
                throw new Exception(e.StackTrace);
            }
        }
    }
}
