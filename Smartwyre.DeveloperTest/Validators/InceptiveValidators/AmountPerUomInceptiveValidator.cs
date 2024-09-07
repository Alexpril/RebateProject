using FluentValidation;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Validators.InceptiveValidators
{
    public class AmountPerUomInceptiveValidator : AbstractValidator<Rebate>
    {
        public AmountPerUomInceptiveValidator(Product product, CalculateRebateRequest request)
        {
            RuleFor(r => product)
                .NotNull().WithMessage("Product must not be null for AmountPerUom.");

            RuleFor(r => r.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero for AmountPerUom.");

            RuleFor(r => request.Volume)
                .GreaterThan(0).WithMessage("Volume must be greater than zero for AmountPerUom.");
            RuleFor(product => product).NotNull();
        }
    }
}
