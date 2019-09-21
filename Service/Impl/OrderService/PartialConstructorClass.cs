using Microsoft.Extensions.Logging;
using SwaggerApp.Data;

namespace SwaggerApp.Service.Impl.OrderService
{
    public partial class OrderService : IOrderService
    {
        private IOrderRepository _orderRepository;
        private ILogger<OrderService> _logger;
        public OrderService(IOrderRepository orderRepository, ILogger<OrderService>  logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }
        
        
    }
}
