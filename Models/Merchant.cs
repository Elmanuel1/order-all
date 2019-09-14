using FluentValidation;
using SwaggerApp.Exceptions;

namespace SwaggerApp.Models
{

    public class Merchant
    {
        public string MerchantId { get; set; }
        public string MerchantName { get; set; }
        public Address Addresses { get; set; }
        public string WorkPhone { get; set; }
    }


    public class MerchantValidator : AbstractValidator<Merchant>
    {
        public MerchantValidator()
        {

            RuleFor(x => x.MerchantId)
                .Length(0, 10)
                .NotEmpty().WithMessage("Merchant Id is required for this transaction. Please review!")
                .OnAnyFailure(merchant => throw new RequestValidationException($"Value '{merchant.MerchantId}' is not valid. Please review!"));

            RuleFor(x => x.MerchantName)
                .Length(0, 10)
                .NotEmpty().WithMessage("Merchant Name is required for this transaction. Please review!")
                .OnAnyFailure(merchant => throw new RequestValidationException($"Value '{merchant.MerchantName}' is not valid. Please review!"));
        }
    }
}
