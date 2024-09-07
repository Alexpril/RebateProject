using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Helpers;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Validators;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Smartwyre.DeveloperTest.Tests")]
namespace Smartwyre.DeveloperTest.Services
{
    public class RebateService : IRebateService
    {
        internal IRebateDataStore _rebateDataStore;
        internal IProductDataStore _productDataStore;
        internal Dictionary<IncentiveType, IRebateCalculator> _incentiveCalculators;

        public RebateService(IProductDataStore productDataStore, IRebateDataStore rebateDataStore)
        {
            _rebateDataStore = rebateDataStore;
            _productDataStore = productDataStore;

            _incentiveCalculators = new Dictionary<IncentiveType, IRebateCalculator>
            {
                { IncentiveType.FixedCashAmount, new FixedCashAmountCalculator() },
                { IncentiveType.FixedRateRebate, new FixedRateRebateCalculator() },
                { IncentiveType.AmountPerUom, new AmountPerUomCalculator() }
            };
        }

        public async Task<CalculateRebateResult> CalculateAsync(CalculateRebateRequest request)
        {
            var result = new CalculateRebateResult();

            Rebate rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
            Product product = _productDataStore.GetProduct(request.ProductIdentifier);

            // Validate
            if (rebate is null)
            {
                return result;
            }

            var validator = new RebateValidator(product, request);
            var validationResult = await validator.ValidateAsync(rebate);

            if(!validationResult.IsValid)
            {
                return result;
            }

            // Calculate
            var calculator = _incentiveCalculators[rebate.Incentive];
            if (calculator.Calculate(rebate, product, request, out decimal rebateAmount))
            {
                _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);
                result.Success = true;
            }

            return result;
        }
    }
}
