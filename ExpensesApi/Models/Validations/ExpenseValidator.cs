using ExpensesApi.Models.Dtos;
using FluentValidation;

namespace ExpensesApi.Models.Validations
{
    /// <summary>
    /// Validation Entity Class
    /// </summary>
    public class ExpenseValidator:AbstractValidator<ExpenseDto>
    {
        public ExpenseValidator()
        {
            RuleFor(e => e.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");
        }
    }
}
