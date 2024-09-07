using FluentValidation.TestHelper;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Validators.InceptiveValidators;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Validators
{
    public class FixedRateRebateInceptiveValidatorTests
    {
        private readonly FixedRateRebateInceptiveValidator _validator;
        private readonly Product _product;
        private readonly CalculateRebateRequest _request;

        public FixedRateRebateInceptiveValidatorTests()
        {
            _product = new Product { Price = 1 };
            _request = new CalculateRebateRequest { Volume = 1 };
            _validator = new FixedRateRebateInceptiveValidator(_product, _request);
        }

        [Fact]
        public void Should_Have_Error_When_Product_Is_Null()
        {
            // Arrange
            var rebate = new Rebate
            {
                Percentage = 10
            };

            // Act
            var validatorWithNullProduct = new FixedRateRebateInceptiveValidator(null, _request);
            var result = validatorWithNullProduct.TestValidate(rebate);

            // Assert
            result.ShouldHaveAnyValidationError()
                  .WithErrorMessage("Product must not be null for FixedRateRebate.");
        }

        [Fact]
        public void Should_Have_Error_When_Percentage_Is_Zero()
        {
            // Arrange
            var rebate = new Rebate
            {
                Percentage = 0
            };

            // Act
            var result = _validator.TestValidate(rebate);

            // Assert
            result.ShouldHaveAnyValidationError()
                  .WithErrorMessage("Percentage must be greater than zero for FixedRateRebate.");
        }

        [Fact]
        public void Should_Have_Error_When_Product_Price_Is_Zero()
        {
            // Arrange
            _product.Price = 0;

            var rebate = new Rebate
            {
                Percentage = 10
            };

            // Act
            var result = _validator.TestValidate(rebate);

            // Assert
            result.ShouldHaveAnyValidationError()
                  .WithErrorMessage("Product price must be greater than zero.");
        }

        [Fact]
        public void Should_Have_Error_When_Volume_Is_Zero()
        {
            // Arrange
            _request.Volume = 0;

            var rebate = new Rebate
            {
                Percentage = 10
            };

            // Act
            var result = _validator.TestValidate(rebate);

            // Assert
            result.ShouldHaveAnyValidationError()
                  .WithErrorMessage("Volume must be greater than zero.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Values_Are_Valid()
        {
            // Arrange
            _product.Price = 10;
            _request.Volume = 5;

            var rebate = new Rebate
            {
                Percentage = 10
            };

            // Act
            var result = _validator.TestValidate(rebate);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}