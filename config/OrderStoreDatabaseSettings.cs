namespace SwaggerApp.config
{
    public class OrderStoreDatabaseSettings : IOrderStoreDatabaseSettings
    {
        public string OrdersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IOrderStoreDatabaseSettings
    {
        string OrdersCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}