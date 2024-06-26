using ExpensesApi.Models.Dtos;
using FluentValidation;

namespace ExpensesApi.Models.Validations
{
    public class CategoryValidator : AbstractValidator<CategoryDto>
    {
        public CategoryValidator() 
        {
            RuleFor(e => e.Name).NotEmpty().WithMessage("Password is required");
            RuleFor(e => e.Description).NotEmpty().WithMessage("UserName is required");        
        }
    }  
}
