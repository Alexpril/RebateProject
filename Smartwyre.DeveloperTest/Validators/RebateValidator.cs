using FluentValidation;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Validators.InceptiveValidators;
using System.Collections.Generic;

namespace Smartwyre.DeveloperTest.Validators
{
    public class RebateValidator : AbstractValidator<Rebate>
    {
        private readonly Dictionary<IncentiveType, IValidator<Rebate>> _inceptiveValidators;

        public RebateValidator(Product product, CalculateRebateRequest request)
        {
            _inceptiveValidators = new Dictionary<IncentiveType, IValidator<Rebate>>
            {
                { IncentiveType.FixedCashAmount, new FixedCashAmountInceptiveValidator() },
                { IncentiveType.FixedRateRebate, new FixedRateRebateInceptiveValidator(product, request) },
                { IncentiveType.AmountPerUom, new AmountPerUomInceptiveValidator(product, request) }
            };

            RuleFor(r => r)
                .NotNull();

            RuleFor(r => r.Incentive)
                .NotNull().Must((rebate, incentiveType) =>
                {
                    var incentiveFlag = (SupportedIncentiveType)(1 << ((int)incentiveType));
                    return (product.SupportedIncentives & incentiveFlag) == incentiveFlag;
                });

            RuleFor(r => r)
                .Custom((rebate, context) => {
                    Include(_inceptiveValidators[rebate.Incentive]);
                });
        }
    }

}
