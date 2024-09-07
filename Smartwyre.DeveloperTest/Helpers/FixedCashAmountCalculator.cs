using Smartwyre.DeveloperTest.Types;
using System;
namespace Smartwyre.DeveloperTest.Helpers
{
    public class FixedCashAmountCalculator : IRebateCalculator
    {
        public bool Calculate(Rebate rebate, Product product, CalculateRebateRequest request, out decimal rebateAmount)
        {
            rebateAmount = rebate.Amount;
            return true;
        }
    }
}
