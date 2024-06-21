using ExpensesApi.Models.Dtos;
using FluentValidation;

namespace ExpensesApi.Models.Validations
{
    public class BudgetValidator : AbstractValidator<BudgetDto>
    {
        public BudgetValidator()
        {
            RuleFor(e => e.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");
            RuleFor(e => e.Name).NotEmpty().WithMessage("Name is required");
        }
    }
}
