using Microsoft.Extensions.Configuration;
using TomadaStore.Utils.Factories.Interfaces;

namespace TomadaStore.Utils.Factories
{
    internal class MongoDBConnectionImpl : IDBConnection
    {
        private readonly string _connectionString;
        private readonly IConfiguration configuration;

        public MongoDBConnectionImpl()
        {
            _connectionString = configuration.GetConnectionString("MongoDBAtlas");
        }

        public string ConnectionString()
        {
            return _connectionString;
        }
    }
}