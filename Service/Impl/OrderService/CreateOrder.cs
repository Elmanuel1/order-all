using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SwaggerApp.Data.Models;
using SwaggerApp.Exceptions;
using SwaggerApp.Models;
using SwaggerApp.vo;

namespace SwaggerApp.Service.Impl.OrderService
{
    public partial class OrderService
    {
        

        public async Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequestRequest)
        {
            if (orderRequestRequest != null)
            {
                bool isValidOrder =  await OrdersUtil.ValidateOrderAsync(orderRequestRequest);
                if (isValidOrder)
                {
                    //log that it is a valid order that is place
                    //save the order that was placed
                    OrderLog orderLog = OrdersUtil.MapOrderRequestToLog(orderRequestRequest);
                    return OrdersUtil.MapOrderLogToOrderResponse( _orderRepository.CreateAsync(orderLog));
                }
            }

            throw new BadRequestException();
        }

        

        

        

        
    }
}
