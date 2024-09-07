using Smartwyre.DeveloperTest.Helpers;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Helpers
{
    public class FixedCashAmountCalculatorTests
    {
        private readonly FixedCashAmountCalculator _calculator;

        public FixedCashAmountCalculatorTests()
        {
            _calculator = new FixedCashAmountCalculator();
        }

        [Fact]
        public void Calculate_Should_Return_True_And_Set_RebateAmount_To_Rebate_Amount()
        {
            // Arrange
            var rebate = new Rebate { Amount = 100 };
            var product = new Product();
            var request = new CalculateRebateRequest();

            // Act
            var result = _calculator.Calculate(rebate, product, request, out var rebateAmount);

            // Assert
            Assert.True(result);
            Assert.Equal(rebate.Amount, rebateAmount);
        }
    }
}