using System.Threading.Tasks;

namespace SwaggerApp.Service
{
    public interface IOrderService
    {
        Task CreateOrderAsync();
        Task GetAllOrdersAsync();
        Task GetOrderByIdAsync();
        Task SearchActiveOrdersAsync();
        Task ActivateRecurringOrderAsync();
    }
}
