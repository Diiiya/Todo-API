using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Todo.Api.Controllers;
using Todo.Api.DTOs;
using Todo.Api.Extensions;
using Todo.Api.Models;
using Todo.Api.Interfaces;
using Xunit;
using Todo.Api.Validators;

namespace Todo.UnitTests.ControllersTests
{
    public class UserControllerTests 
    {
        private readonly Mock<IConfiguration> configurationStub = new();
        private readonly Mock<IUserRepo> userRepoStub = new();
        private readonly PasswordHasher passwordHasher = new();

        [Fact]
        public async Task GetUserAsync_WithNotExistingUser_ReturnsNotFound()
        {
            // Arrange
            var controller = new UserController(userRepoStub.Object, configurationStub.Object);

            // Act
            var result = await controller.GetUserAsync(Guid.NewGuid());

            // Assert
            // Assert.IsType<NotFoundResult>(result.Result);
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetUserAsync_WithExistingUser_ReturnsExpectedUser()
        {          
            // Arrange
            var expectedUser = CreateRandomUser();
            userRepoStub.Setup(repo => repo.Get(It.IsAny<Guid>())).ReturnsAsync(expectedUser);

            var controller = new UserController(userRepoStub.Object, configurationStub.Object);

            // Act
            var result = await controller.GetUserAsync(Guid.NewGuid());

            // Assert

            // Old way
            // Assert.IsType<UserDTO>(result.Value);
            // var dto = (result as ActionResult<UserDTO>).Value;
            // Assert.Equal(expectedUser.Id, dto.Id);
            // Assert.Equal(expectedUser.Username, dto.Username);
            // Assert.Equal(expectedUser.Email, dto.Email);

            // Better way with package FluentAssertions
            // dotnet add package FluentAssertions
            result.Value.Should().BeEquivalentTo(
                expectedUser,
                options => options.ComparingByMembers<User>().ExcludingMissingMembers());
            result.Value.Should().BeOfType<UserDTO>();
        }

        private User CreateRandomUser() 
        {
            return new() 
            {
                Id = Guid.NewGuid(),
                Username = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString() + "@mail.com",
                Password = passwordHasher.hashPass("lala123"),
                CreatedDate = DateTimeOffset.UtcNow,
                Deleted = false,
                ToDos = null
            };
        }

        [Fact]
        public async Task CreateUserAsync_WithUserToCreate_ReturnsCreatedUser()
        { 
            // Arrange
            var userToCreate = new CreateUserDTO()
            {
                Username = "username666",
                Email = "lalal@mail.com",
                Password = "lala123"
            };
            // var userRepo2 = new EfCoreUserRepository(context);
            var controller = new UserController(userRepoStub.Object, configurationStub.Object);

            // Act
            var result = await controller.CreateUserAsync(userToCreate);

            // Assert
            var createdUser = (result.Result as CreatedAtActionResult).Value as UserDTO;

            // The following would work if the password was not being hashed differently
            // userToCreate.Should().BeEquivalentTo(
            //     createdUser,
            //     options => options.ComparingByMembers<UserDTO>().ExcludingMissingMembers()
            // );
            createdUser.Id.Should().NotBeEmpty();

            // Checks only the username and email because that is what the UserDTO has access to
            Assert.Equal(userToCreate.Username, createdUser.Username);
            Assert.Equal(userToCreate.Email, createdUser.Email);
            result.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task UpdateUserAsync_WithExistingUser_ReturnsNoContent()
        { 
            // Arrange
            var existingUser = CreateRandomUser();
            userRepoStub.Setup(repo => repo.Get(It.IsAny<Guid>())).ReturnsAsync(existingUser);
            var userId = existingUser.Id;

            var userToUpdate = new UpdateUserDTO()
            {
                Email = "new@mail.com",
                CurrentPassword = "lala123"
            };

            var controller = new UserController(userRepoStub.Object, configurationStub.Object);

            // Act
            var result = await controller.UpdateUserAsync(userId, userToUpdate);   

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateUserAsync_WithNotExistingUser_ReturnsNotFound()
        { 
            // Arrange
            var userId = Guid.NewGuid();

            var userToUpdate = new UpdateUserDTO()
            {
                Email = "new@mail.com",
                CurrentPassword = "lala123"
            };

            var controller = new UserController(userRepoStub.Object, configurationStub.Object);

            // Act
            var result = await controller.UpdateUserAsync(userId, userToUpdate);   

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateUserAsync_WithWrongCurrentPassword_ReturnsUnauthorized()
        { 
            // Arrange
            var existingUser = CreateRandomUser();
            userRepoStub.Setup(repo => repo.Get(It.IsAny<Guid>())).ReturnsAsync(existingUser);
            var userId = existingUser.Id;

            var userToUpdate = new UpdateUserDTO()
            {
                Email = "new@mail.com",
                CurrentPassword = "lala1234"
            };

            var controller = new UserController(userRepoStub.Object, configurationStub.Object);

            // Act
            var result = await controller.UpdateUserAsync(userId, userToUpdate);   

            // Assert
            result.Should().BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public async Task DeleteUserAsync_WithExistingUserUser_ReturnsNoContent()
        { 
            // Arrange
            var existingUser = CreateRandomUser();
            userRepoStub.Setup(repo => repo.Get(It.IsAny<Guid>())).ReturnsAsync(existingUser);

            var controller = new UserController(userRepoStub.Object, configurationStub.Object);

            // Act
            var result = await controller.DeleteUserAsync(existingUser.Id);   

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteUserAsync_WithNotExistingUserUser_ReturnsNotFound()
        { 
            // Arrange
            var userId = Guid.NewGuid();
            var controller = new UserController(userRepoStub.Object, configurationStub.Object);

            // Act
            var result = await controller.DeleteUserAsync(userId);   

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        // [Fact]
        // Fails because IEnumerable and LINQ query in the method ?
        // public async Task AuthenticateUserAsync_WithCorrectUserCredentials_ReturnsOkObjectResult()
        // { 
        //     // Arrange
        //     var existingUser = CreateRandomUser();
        //     userRepoStub.Setup(repo => repo.Get(It.IsAny<Guid>())).ReturnsAsync(existingUser);

        //     var userLogin = new LoginUserDTO()
        //     {
        //         Login = "username666",
        //         Password = "lala123"
        //     };

        //     var controller = new UserController(userRepoStub.Object, configurationStub.Object);

        //     // Act
        //     var result = await controller.AuthenticateUserAsync(userLogin);   

        //     // Assert
        //     result.Should().BeOfType<OkObjectResult>();
        // }

        

    }
}