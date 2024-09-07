using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Helpers
{
    public class AmountPerUomCalculator : IRebateCalculator
    {
        public bool Calculate(Rebate rebate, Product product, CalculateRebateRequest request, out decimal rebateAmount)
        {
            rebateAmount = rebate.Amount * request.Volume;
            return true;
        }
    }
}
