using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SwaggerApp.Data.Models;
using SwaggerApp.Models;
using SwaggerApp.vo;

namespace SwaggerApp.Service
{
    public class OrdersUtil
    {
        public string OrderNumber { get; set; }   //not validated
        public DateTime ExpectedDateOfDelivery { get; set; }
        public string  MerchantId { get; set; }  //validated
        public List<OrderItem> OrderItems { get; set; } //validated
        public DateTime DateOrdered { get; set; }   //not validated
        public String  UserId { get; set; }    //gotten from the token
        public bool UseDefaultLocation { get; } = false; //validated
        public PaymentDetail PaymentDetail { get; set; }   
        public bool ActivateRecurringOrder { get; set; }
        public DateTime DateRecurringIsActive { get; set; } //required if ActivateRecurringOrder is true
        public int MonthDuration { get; set; }  //required if ActivateRecurringOrder is true
        public bool canRemind { get; set; }  //required if ActivateRecurringOrder is true
        public static async Task<bool> ValidateOrderAsync(OrderRequest orderRequest)
        {
            if (orderRequest != null)
            {
                ValidateUser();
                ValidateMerchant();
                ValidateOrderItems();
                ValidatePaymentDetails();
            }
            return await Task.FromResult(true);
        }

        private static void ValidatePaymentDetails()
        {
            throw new NotImplementedException();
        }

        private static void ValidateOrderItems()
        {
            throw new NotImplementedException();
        }

        private static void ValidateMerchant()
        {
            throw new NotImplementedException();
        }

        private static void ValidateUser()
        {
            throw new NotImplementedException();
        }

        public static OrderLog MapOrderRequestToLog(OrderRequest orderRequestRequest)
        {
            throw new NotImplementedException();
        }

        public static OrderResponse MapOrderLogToOrderResponse(OrderLog createAsync)
        {
            throw new NotImplementedException();
        }
        
        
    }
}