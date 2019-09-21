using System.Threading.Tasks;
using SwaggerApp.Data.Models;
using SwaggerApp.Models;
using SwaggerApp.vo;

namespace SwaggerApp.Service
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest);
        Task GetAllOrdersAsync();
        Task GetOrderByIdAsync();
        Task SearchActiveOrdersAsync();
        Task ActivateRecurringOrderAsync();
    }
}
