using System;
using System.Collections.Generic;
using FluentValidation;
using SwaggerApp.Exceptions;
using SwaggerApp.Models;

namespace SwaggerApp.vo
{
    public class OrderRequest 
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
    }

    public class OrderValidator : AbstractValidator<OrderRequest>
    {
        public OrderValidator()
        {
            RuleFor(x => x.UseDefaultLocation)
                .NotNull()
                .OnFailure(order => throw new RequestValidationException($"Value {order.UseDefaultLocation}  is not valid. Please review!"));

            //RuleFor(x => x.Merchant).SetValidator(new MerchantValidator());
            //RuleForEach(order => order.OrderItems).SetValidator(new OrderItemValidator());
        }
    }


}
