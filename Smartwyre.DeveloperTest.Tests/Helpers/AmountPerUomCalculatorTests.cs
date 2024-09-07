using Smartwyre.DeveloperTest.Helpers;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Helpers
{
    public class AmountPerUomCalculatorTests
    {
        private readonly AmountPerUomCalculator _calculator;

        public AmountPerUomCalculatorTests()
        {
            _calculator = new AmountPerUomCalculator();
        }

        [Fact]
        public void Calculate_Should_Return_True_And_Correct_RebateAmount_When_Valid_Input()
        {
            // Arrange
            var rebate = new Rebate { Amount = 10 };
            var product = new Product(); // Product is not used in calculation, but needs to be provided
            var request = new CalculateRebateRequest { Volume = 5 };

            // Act
            var result = _calculator.Calculate(rebate, product, request, out var rebateAmount);

            // Assert
            Assert.True(result);
            Assert.Equal(50, rebateAmount);
        }
    }
}
