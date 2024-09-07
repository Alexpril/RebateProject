using Smartwyre.DeveloperTest.Helpers;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Helpers
{
    public class FixedRateRebateCalculatorTests
    {
        private readonly FixedRateRebateCalculator _calculator;

        public FixedRateRebateCalculatorTests()
        {
            _calculator = new FixedRateRebateCalculator();
        }

        [Fact]
        public void Calculate_Should_Return_True_And_Correct_RebateAmount_When_Valid_Input()
        {
            // Arrange
            var rebate = new Rebate { Percentage = 0.1m };
            var product = new Product { Price = 200 };
            var request = new CalculateRebateRequest { Volume = 5 };

            // Act
            var result = _calculator.Calculate(rebate, product, request, out var rebateAmount);

            // Assert
            Assert.True(result);
            Assert.Equal(100, rebateAmount);
        }
    }
}