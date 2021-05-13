using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Todo.Api.Controllers;
using Todo.Api.Data;
using Todo.Api.Data.EfCore;
using Xunit;

namespace Todo.UnitTests
{
    public class UserControllerTests : DataContextForTests
    {
        public UserControllerTests(): base(new DbContextOptionsBuilder<DataContext>().UseSqlite("Filename=Test.db").Options)
        {

        }

        [Fact]
        public async Task GetUserAsync_NotExistingUser_ReturnsNotFound()
        {
            using (var context = new DataContext(ContextOptions))
            {
                // Arrange
                var configurationStub = new Mock<IConfiguration>();
                var userRepo = new EfCoreUserRepository(context);
                var controller = new UserController(userRepo, configurationStub.Object);

                // Act
                var result = await controller.GetUserAsync(Guid.NewGuid());

                // Assert
                Assert.IsType<NotFoundResult>(result.Result);
            }
        }

        [Fact]
        public async Task GetAllUsersAsync_CountsAllUsers_ReturnsTwo()
        {
            using (var context = new DataContext(ContextOptions))
            {
                // Arrange
                var configurationStub = new Mock<IConfiguration>();
                var userRepo = new EfCoreUserRepository(context);
                var controller = new UserController(userRepo, configurationStub.Object);

                // Act
                var result = await controller.GetAllUsersAsync();
                var expectedUserCount = 2;
                var actualUserCount = 0;
                foreach (var item in result)
                {
                    actualUserCount++;
                }

                // Assert
                Assert.Equal(expectedUserCount, actualUserCount);
            }
        }
    }
}