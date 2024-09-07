using Smartwyre.DeveloperTest.Types;
using System;
namespace Smartwyre.DeveloperTest.Helpers
{
    public class FixedRateRebateCalculator : IRebateCalculator
    {
        public bool Calculate(Rebate rebate, Product product, CalculateRebateRequest request, out decimal rebateAmount)
        {
            rebateAmount = product.Price * rebate.Percentage * request.Volume;
            return true;
        }
    }
}
