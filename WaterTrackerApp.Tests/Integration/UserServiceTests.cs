using System;
using System.Threading.Tasks;
using Xunit;
using WaterTrackerApp.Application.Dtos;
using WaterTrackerApp.Infrastructure.Services;
using WaterTrackerApp.Tests.Utilities;
using System.Linq;

namespace WaterTrackerApp.Tests.Integration
{
    public class UserServiceTests
    {
        [Fact]
        public async Task CreateAsync_AddsUser()
        {
            //arrange
            var context = TestDbContext.CreateInMemoryDbContext();
            var service = new UserService(context);
            var userDto = new UserDto { FirstName = "Test", Surname = "User", Email = "test@example.com" };
            //act
            var result = await service.CreateAsync(userDto);
            //assert
            Assert.NotEqual(0, result.Id);
            Assert.Single(context.Users);
            Assert.Equal("Test", context.Users.First().FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsUser_WhenExists()
        {
            var context = TestDbContext.CreateInMemoryDbContext();
            context.Users.Add(new Domain.Entities.User { FirstName = "Test", Surname = "User", Email = "test@example.com" });
            context.SaveChanges();
            var service = new UserService(context);
            var userId = context.Users.First().Id;

            var result = await service.GetByIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal("Test", result.FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            var context = TestDbContext.CreateInMemoryDbContext();
            var service = new UserService(context);

            var result = await service.GetByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesUser_WhenExists()
        {
            var context = TestDbContext.CreateInMemoryDbContext();
            context.Users.Add(new Domain.Entities.User { FirstName = "Old", Surname = "Name", Email = "old@example.com" });
            context.SaveChanges();
            var service = new UserService(context);
            var userId = context.Users.First().Id;
            var updatedDto = new UserDto { Id = userId, FirstName = "New", Surname = "Name", Email = "new@example.com" };

            var result = await service.UpdateAsync(userId, updatedDto);

            Assert.True(result);
            Assert.Equal("New", context.Users.First().FirstName);
            Assert.Equal("new@example.com", context.Users.First().Email);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenUserNotFound()
        {
            var context = TestDbContext.CreateInMemoryDbContext();
            var service = new UserService(context);
            var updatedDto = new UserDto { Id = 999, FirstName = "No", Surname = "User", Email = "no@user.com" };

            var result = await service.UpdateAsync(999, updatedDto);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_RemovesUser_WhenExists()
        {
            var context = TestDbContext.CreateInMemoryDbContext();
            context.Users.Add(new Domain.Entities.User { FirstName = "Delete", Surname = "Me", Email = "delete@example.com" });
            context.SaveChanges();
            var service = new UserService(context);
            var userId = context.Users.First().Id;

            var result = await service.DeleteAsync(userId);

            Assert.True(result);
            Assert.Empty(context.Users);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenUserNotFound()
        {
            var context = TestDbContext.CreateInMemoryDbContext();
            var service = new UserService(context);

            var result = await service.DeleteAsync(999);

            Assert.False(result);
        }
    }
}
