using FluentValidation.TestHelper;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Validators.InceptiveValidators;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Validators
{
    public class AmountPerUomInceptiveValidatorTests
    {
        private readonly AmountPerUomInceptiveValidator _validator;
        private readonly Product _product;
        private readonly CalculateRebateRequest _request;

        public AmountPerUomInceptiveValidatorTests()
        {
            _product = new Product();
            _request = new CalculateRebateRequest { Volume = 1 };

            _validator = new AmountPerUomInceptiveValidator(_product, _request);
        }

        [Fact]
        public void Should_Have_Error_When_Product_Is_Null()
        {
            // Arrange
            var rebate = new Rebate
            {
                Amount = 10
            };

            // Act
            var validatorWithNullProduct = new AmountPerUomInceptiveValidator(null, _request);
            var result = validatorWithNullProduct.TestValidate(rebate);

            // Assert
            result.ShouldHaveAnyValidationError()
                  .WithErrorMessage("Product must not be null for AmountPerUom.");
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
                  .WithErrorMessage("Amount must be greater than zero for AmountPerUom.");
        }

        [Fact]
        public void Should_Have_Error_When_Volume_Is_Zero()
        {
            // Arrange
            var rebate = new Rebate
            {
                Amount = 10
            };

            var requestWithZeroVolume = new CalculateRebateRequest { Volume = 0 };
            var validatorWithZeroVolume = new AmountPerUomInceptiveValidator(_product, requestWithZeroVolume);
            
            // Act
            var result = validatorWithZeroVolume.TestValidate(rebate);

            // Assert
            result.ShouldHaveAnyValidationError()
                  .WithErrorMessage("Volume must be greater than zero for AmountPerUom.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Amount_And_Volume_Are_Valid()
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