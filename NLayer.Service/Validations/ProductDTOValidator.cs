using FluentValidation;
using NLayer.Core.DTOs;

namespace NLayer.Service.Validations
{
    public class ProductDTOValidator : AbstractValidator<ProductDTO>
    {
        public ProductDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("{PropertyName} is required.")
                .NotEmpty().WithMessage("{PropertyName} is required."); //propertyName  DTO daki Name
            RuleFor(x => x.Price).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater than 0 ");
            RuleFor(x => x.Stock).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater than 0 ");
            RuleFor(x => x.CategoryId).InclusiveBetween(1, int.MaxValue).WithMessage("{PropertyName} must be greater than 0 ");
        }
    }
}