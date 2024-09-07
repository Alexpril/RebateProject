using FluentValidation;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Validators.InceptiveValidators
{
    public class FixedCashAmountInceptiveValidator : AbstractValidator<Rebate>
    {
        public FixedCashAmountInceptiveValidator()
        {
            RuleFor(r => r.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero for FixedCashAmount.");
        }
    }
}
