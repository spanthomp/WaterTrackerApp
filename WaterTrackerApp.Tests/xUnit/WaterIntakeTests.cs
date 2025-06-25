using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using WaterTrackerApp.Application.Interfaces;
using WaterTrackerApp.Application.Dtos;
using WaterTrackerApp.Application.Services;

namespace WaterTrackerApp.Tests.xUnit
{
    public class WaterConsumptionCalculatorTests
    {
        [Fact]
        public async Task GetTotalConsumed_ReturnsSumOfConsumedWater()
        {
            //arrange
            var mockService = new Mock<IWaterIntakeService>();
            mockService.Setup(s => s.GetByUserIdAsync(1))
                .ReturnsAsync(new List<WaterIntakeDto>
                {
                    new WaterIntakeDto { ConsumedWater = 500 },
                    new WaterIntakeDto { ConsumedWater = 700 }
                });

            var calculator = new WaterConsumptionCalculator(mockService.Object);

            //act
            var total = await calculator.GetTotalConsumed(1);

            //assert
            Assert.Equal(1200, total);
        }

        [Fact]
        public async Task GetTotalConsumed_ReturnsZero_WhenNoRecords()
        {
            var mockService = new Mock<IWaterIntakeService>();
            mockService.Setup(s => s.GetByUserIdAsync(2))
                .ReturnsAsync(new List<WaterIntakeDto>());

            var calculator = new WaterConsumptionCalculator(mockService.Object);

            var total = await calculator.GetTotalConsumed(2);

            Assert.Equal(0, total);
        }
    }
}