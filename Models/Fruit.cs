using FluentValidation;
using SwaggerApp.Exceptions;

namespace SwaggerApp.Models
{
    public class Fruit
    {
        public int Id { get; set; }

   
        public string Name { get; set; }
    }

    public class FruitValidator : AbstractValidator<Fruit>
    {
        public FruitValidator()
        {

            RuleFor(x => x.Name)
                .Length(0, 10)
                .NotEmpty().WithMessage("Name is required. Please review")
               .OnFailure(fruit => throw new RequestValidationException($"Value {fruit.Name}  is not valid. Please review!"));
        }

       
    }
}
