using TomadaStore.Utils.Factories.Interfaces;

namespace TomadaStore.Utils.Factories
{
    public class MongoDBConnection : DBConnectionFactory
    {
        public override IDBConnection CreateDBConnection()
        {
            return new MongoDBConnectionImpl();
        }
    }
}
