using FluentValidation;
using SwaggerApp.Exceptions;

namespace SwaggerApp.Models
{
    public class OrderItem
    {
        public string OrderItemId { get; set; }
        public string ItemName { get; set; }          
        public int Price { get; set; }
        private double ItemPrice { get; set; }
        public int NoOfItemOrdered { get; set; }

    }

    public class OrderItemValidator : AbstractValidator<OrderItem>
    {
        public OrderItemValidator()
        {

            RuleFor(x => x.NoOfItemOrdered)
                .NotEmpty().WithMessage("Item should be more than Zero to complete this transaction. Please review!")
                .Must(x=>x > 0).WithMessage("Item ordered should be greater more than 0. Please review!")
                .OnAnyFailure(orderItem => throw new RequestValidationException($"Value {orderItem.NoOfItemOrdered}  is not valid for NoOfItem. Please review!"));

            RuleFor(x => x.OrderItemId)
                .Length(0, 10)
                .NotEmpty().WithMessage("Order Id is required for this transaction. Please review!")
                .OnAnyFailure(orderItem => throw new RequestValidationException($"Value {orderItem.OrderItemId} as OrderItemId is not valid. Please review!"));
        }
    }





}
