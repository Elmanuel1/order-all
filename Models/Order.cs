using System;
using System.Collections.Generic;
using FluentValidation;
using SwaggerApp.Exceptions;

namespace SwaggerApp.Models
{
    public class Order 
    {
       // public string OrderNumber { get; set; }   //not validated
        public Merchant Merchant { get; set; }  //validated
        public List<OrderItem> OrderItems { get; set; } //validated
        //public DateTime DateOrdered { get; set; }   //not validated
        //public User User { get; set; }    //gotten from the token
        //public double TotalPrice { get; set; }  //not validated
        public bool UseDefaultLocation { get; } = false; //validated
        public PaymentDetail PaymentDetail { get; set; }   
    }

    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {

            RuleFor(x => x.UseDefaultLocation)
                .NotNull()
                .OnFailure(order => throw new RequestValidationException($"Value {order.UseDefaultLocation}  is not valid. Please review!"));

            RuleFor(x => x.Merchant).SetValidator(new MerchantValidator());
            RuleForEach(order => order.OrderItems).SetValidator(new OrderItemValidator());
        }
    }


}
