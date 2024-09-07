using FluentValidation;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Validators.InceptiveValidators
{
    public class FixedRateRebateInceptiveValidator : AbstractValidator<Rebate>
    {
        public FixedRateRebateInceptiveValidator(Product product, CalculateRebateRequest request)
        {
            RuleFor(r => product)
                .NotNull().WithMessage("Product must not be null for FixedRateRebate.");

            RuleFor(r => r.Percentage)
                .GreaterThan(0).WithMessage("Percentage must be greater than zero for FixedRateRebate.");

            When(r => product != null, () =>
            {
                RuleFor(r => product.Price)
                    .GreaterThan(0).WithMessage("Product price must be greater than zero.");
            });

            RuleFor(r => request.Volume)
                .GreaterThan(0).WithMessage("Volume must be greater than zero.");
        }
    }
}
