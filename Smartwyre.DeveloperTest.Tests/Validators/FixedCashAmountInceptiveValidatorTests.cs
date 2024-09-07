using FluentValidation.TestHelper;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Validators.InceptiveValidators;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Validators
{
    public class FixedCashAmountInceptiveValidatorTests
    {
        private readonly FixedCashAmountInceptiveValidator _validator;

        public FixedCashAmountInceptiveValidatorTests()
        {
            _validator = new FixedCashAmountInceptiveValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Amount_Is_Zero()
        {
            // Arrange
            var rebate = new Rebate
            {
                Amount = 0
            };

            // Act
            var result = _validator.TestValidate(rebate);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Amount)
                  .WithErrorMessage("Amount must be greater than zero for FixedCashAmount.");
        }

        [Fact]
        public void Should_Have_Error_When_Amount_Is_Negative()
        {
            // Arrange
            var rebate = new Rebate
            {
                Amount = -1
            };

            // Act
            var result = _validator.TestValidate(rebate);

            // Assert
            result.ShouldHaveValidationErrorFor(r => r.Amount)
                  .WithErrorMessage("Amount must be greater than zero for FixedCashAmount.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Amount_Is_Positive()
        {
            // Arrange
            var rebate = new Rebate
            {
                Amount = 10
            };

            // Act
            var result = _validator.TestValidate(rebate);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}