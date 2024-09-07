using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Helpers
{
    public interface IRebateCalculator
    {
        bool Calculate(Rebate rebate, Product product, CalculateRebateRequest request, out decimal rebateAmount);
    }
}
