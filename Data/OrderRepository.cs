using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using SwaggerApp.config;
using SwaggerApp.Data.Models;

namespace SwaggerApp.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<OrderLog> _orders;
        public OrderRepository(IOrderStoreDatabaseSettings settings, IMongoDatasource datasource)
        {
            IMongoDatabase database = datasource.GetClient().GetDatabase(settings.DatabaseName);
            _orders = database.GetCollection<OrderLog>(settings.OrdersCollectionName);
        }

        public OrderLog CreateAsync(OrderLog orderLog)
        {
            _orders.InsertOneAsync(orderLog);
            return orderLog;
        }
        public Task<List<OrderLog>> GetAsync() =>
            _orders.Find(book => true).ToListAsync();

        public Task<OrderLog> GetAsync(string id) =>
            _orders.Find(order => order.OrderId == id).FirstOrDefaultAsync();
        
        public void UpdateAsync(string id, OrderLog orderLogIn) =>
            _orders.ReplaceOneAsync(order => order.OrderId == id, orderLogIn);

        public void RemoveAsync(OrderLog orderLogIn) =>
            _orders.DeleteOneAsync(order => order.OrderId == orderLogIn.OrderId);

        public void RemoveAsync(string id) => 
            _orders.DeleteOneAsync(book => book.OrderId == id);
    }

    public interface IOrderRepository
    {
        OrderLog CreateAsync(OrderLog book);
        Task<List<OrderLog>> GetAsync();

        Task<OrderLog> GetAsync(string id);

        void UpdateAsync(string id, OrderLog orderLogIn);
        void RemoveAsync(OrderLog orderLogIn);
        void RemoveAsync(string id);
    }
}