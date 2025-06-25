using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using WaterTrackerApp.Application.Dtos;
using WaterTrackerApp.Infrastructure.Services;
using WaterTrackerApp.Tests.Utilities;

namespace WaterTrackerApp.Tests.Integration
{
    public class WaterIntakeServiceTests
    {
        [Fact]
        public async Task CreateAsync_AddsWaterIntake()
        {
            //arrange
            var context = TestDbContext.CreateInMemoryDbContext();
            var service = new WaterIntakeService(context);
            var dto = new WaterIntakeDto
            {
                UserId = 1,
                IntakeDate = DateTime.Today,
                ConsumedWater = 500
            };
            //act
            var result = await service.CreateAsync(dto);
            //assert
            Assert.NotEqual(0, result.Id);
            Assert.Single(context.WaterIntakes);
            Assert.Equal(500, context.WaterIntakes.First().ConsumedWater);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsRecord_WhenExists()
        {
            var context = TestDbContext.CreateInMemoryDbContext();
            context.WaterIntakes.Add(new WaterTrackerApp.Domain.Entities.WaterIntake
            {
                UserId = 1,
                IntakeDate = DateTime.Today,
                ConsumedWater = 750
            });
            context.SaveChanges();
            var service = new WaterIntakeService(context);
            var id = context.WaterIntakes.First().Id;

            var result = await service.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(750, result.ConsumedWater);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            var context = TestDbContext.CreateInMemoryDbContext();
            var service = new WaterIntakeService(context);

            var result = await service.GetByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByUserIdAsync_ReturnsAllRecordsForUser()
        {
            var context = TestDbContext.CreateInMemoryDbContext();
            context.WaterIntakes.AddRange(
                new WaterTrackerApp.Domain.Entities.WaterIntake { UserId = 1, IntakeDate = DateTime.Today, ConsumedWater = 300 },
                new WaterTrackerApp.Domain.Entities.WaterIntake { UserId = 1, IntakeDate = DateTime.Today, ConsumedWater = 400 },
                new WaterTrackerApp.Domain.Entities.WaterIntake { UserId = 2, IntakeDate = DateTime.Today, ConsumedWater = 500 }
            );
            context.SaveChanges();
            var service = new WaterIntakeService(context);

            var result = (await service.GetByUserIdAsync(1)).ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal(1, r.UserId));
        }

        [Fact]
        public async Task UpdateAsync_UpdatesRecord_WhenExists()
        {
            var context = TestDbContext.CreateInMemoryDbContext();
            context.WaterIntakes.Add(new WaterTrackerApp.Domain.Entities.WaterIntake
            {
                UserId = 1,
                IntakeDate = DateTime.Today,
                ConsumedWater = 200
            });
            context.SaveChanges();
            var service = new WaterIntakeService(context);
            var id = context.WaterIntakes.First().Id;
            var updatedDto = new WaterIntakeDto
            {
                Id = id,
                UserId = 1,
                IntakeDate = DateTime.Today,
                ConsumedWater = 800
            };

            var result = await service.UpdateAsync(id, updatedDto);

            Assert.True(result);
            Assert.Equal(800, context.WaterIntakes.First().ConsumedWater);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenNotExists()
        {
            var context = TestDbContext.CreateInMemoryDbContext();
            var service = new WaterIntakeService(context);
            var updatedDto = new WaterIntakeDto
            {
                Id = 999,
                UserId = 1,
                IntakeDate = DateTime.Today,
                ConsumedWater = 800
            };

            var result = await service.UpdateAsync(999, updatedDto);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_RemovesRecord_WhenExists()
        {
            var context = TestDbContext.CreateInMemoryDbContext();
            context.WaterIntakes.Add(new WaterTrackerApp.Domain.Entities.WaterIntake
            {
                UserId = 1,
                IntakeDate = DateTime.Today,
                ConsumedWater = 100
            });
            context.SaveChanges();
            var service = new WaterIntakeService(context);
            var id = context.WaterIntakes.First().Id;

            var result = await service.DeleteAsync(id);

            Assert.True(result);
            Assert.Empty(context.WaterIntakes);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotExists()
        {
            var context = TestDbContext.CreateInMemoryDbContext();
            var service = new WaterIntakeService(context);

            var result = await service.DeleteAsync(999);

            Assert.False(result);
        }
    }
}