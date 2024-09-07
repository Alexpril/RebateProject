using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Helpers;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Smartwyre.DeveloperTest.Validators;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Services
{
    public class RebateServiceTests
    {
        private readonly RebateService _rebateService;
        private readonly Mock<IRebateDataStore> _mockRebateDataStore;
        private readonly Mock<IProductDataStore> _mockProductDataStore;
        private readonly Mock<IRebateCalculator> _mockFixedCashAmountCalculator;
        private readonly Mock<IRebateCalculator> _mockFixedRateRebateCalculator;
        private readonly Mock<IRebateCalculator> _mockAmountPerUomCalculator;
        private readonly Mock<RebateValidator> _mockRebateValidator;

        public RebateServiceTests()
        {
            _mockRebateDataStore = new Mock<IRebateDataStore>();
            _mockProductDataStore = new Mock<IProductDataStore>();
            _mockFixedCashAmountCalculator = new Mock<IRebateCalculator>();
            _mockFixedRateRebateCalculator = new Mock<IRebateCalculator>();
            _mockAmountPerUomCalculator = new Mock<IRebateCalculator>();

            _rebateService = new RebateService(_mockProductDataStore.Object, _mockRebateDataStore.Object)
            {
                _incentiveCalculators = new()
                {
                    { IncentiveType.FixedCashAmount, _mockFixedCashAmountCalculator.Object },
                    { IncentiveType.FixedRateRebate, _mockFixedRateRebateCalculator.Object },
                    { IncentiveType.AmountPerUom, _mockAmountPerUomCalculator.Object }
                }
            };
        }

        [Fact]
        public async Task CalculateAsync_Should_Return_Success_And_Store_Result_When_Validation_Succeeds()
        {
            // Arrange
            var request = new CalculateRebateRequest() { Volume = 1 };
            var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = 100 };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedCashAmount | SupportedIncentiveType.AmountPerUom, Price = 200 };

            _mockRebateDataStore.Setup(x => x.GetRebate(request.RebateIdentifier)).Returns(rebate);
            _mockProductDataStore.Setup(x => x.GetProduct(request.ProductIdentifier)).Returns(product);
            _mockFixedCashAmountCalculator.Setup(x => x.Calculate(rebate, product, request, out It.Ref<decimal>.IsAny)).Returns(true);
            
            // Act
            var result = await _rebateService.CalculateAsync(request);

            // Assert
            Assert.True(result.Success);
            _mockRebateDataStore.Verify(x => x.StoreCalculationResult(rebate, It.Ref<decimal>.IsAny), Times.Once);
        }

        [Fact]
        public async Task CalculateAsync_Should_Return_Failure_When_Validation_Fails()
        {
            // Arrange
            var request = new CalculateRebateRequest();
            var rebate = new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = 100 };

            _mockRebateDataStore.Setup(x => x.GetRebate(request.RebateIdentifier)).Returns(rebate);
            _mockProductDataStore.Setup(x => x.GetProduct(request.ProductIdentifier)).Returns(new Product());

            // Act
            var result = await _rebateService.CalculateAsync(request);

            // Assert
            Assert.False(result.Success);
            _mockRebateDataStore.Verify(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.Ref<decimal>.IsAny), Times.Never);
        }

        [Fact]
        public async Task CalculateAsync_Should_Handle_Calculator_Selection()
        {
            // Arrange
            var request = new CalculateRebateRequest() { Volume = 1 };
            var rebate = new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = 0.1m };
            var product = new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate | SupportedIncentiveType.AmountPerUom, Price = 200 };

            _mockRebateDataStore.Setup(x => x.GetRebate(request.RebateIdentifier)).Returns(rebate);
            _mockProductDataStore.Setup(x => x.GetProduct(request.ProductIdentifier)).Returns(product);
            _mockFixedRateRebateCalculator.Setup(x => x.Calculate(rebate, product, request, out It.Ref<decimal>.IsAny)).Returns(true);

            // Act
            var result = await _rebateService.CalculateAsync(request);

            // Assert
            Assert.True(result.Success);
            _mockFixedRateRebateCalculator.Verify(x => x.Calculate(rebate, product, request, out It.Ref<decimal>.IsAny), Times.Once);
        }

        [Fact]
        public async Task CalculateAsync_Should_Handle_Rebate_And_Product_Not_Found()
        {
            // Arrange
            var request = new CalculateRebateRequest();

            _mockRebateDataStore.Setup(x => x.GetRebate(request.RebateIdentifier)).Returns((Rebate)null);
            _mockProductDataStore.Setup(x => x.GetProduct(request.ProductIdentifier)).Returns((Product)null);

            // Act
            var result = await _rebateService.CalculateAsync(request);

            // Assert
            Assert.False(result.Success);
            _mockRebateDataStore.Verify(x => x.StoreCalculationResult(It.IsAny<Rebate>(), It.Ref<decimal>.IsAny), Times.Never);
        }
    }
}