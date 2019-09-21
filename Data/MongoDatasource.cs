using MongoDB.Driver;
using SwaggerApp.config;

namespace SwaggerApp.Data
{
    
    public class MongoDatasource : IMongoDatasource
    {
        private MongoClient _client;
        public MongoDatasource(IOrderStoreDatabaseSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
        }

        public MongoClient GetClient()
        {
            return _client;
        }
    }

    public interface IMongoDatasource
    {
        MongoClient GetClient();
    }
}