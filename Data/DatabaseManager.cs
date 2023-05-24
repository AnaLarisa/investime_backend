using MongoDB.Driver;

namespace backend.Services
{
    public class DatabaseManager
    {
        private string connectionString;
        private string databaseName;
        private MongoClient client;

        public DatabaseManager(string connectionString, string databaseName)
        {
            this.connectionString = connectionString;
            this.databaseName = databaseName;
            Connect();
        }

        private void Connect()
        {
            client = new MongoClient(connectionString);
        }

        public IMongoDatabase GetDatabase()
        {
            return client.GetDatabase(databaseName);
        }
    }

}
