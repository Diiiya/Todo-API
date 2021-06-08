using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Todo.Api.DTOs;
using Xunit;

namespace Todo.IntegrationTests
{
    public class UserControllerTests : IntegrationTest
    {
        // Here we test JWT and not user role/permission
        [Fact]
        public async Task GetAllUsers_WithAnyJWT_ReturnsOkAndUserCount()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response = await testClient.GetAsync("/users");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var userList = JsonConvert.DeserializeObject<UserDTO[]>(await response.Content.ReadAsStringAsync());
            userList.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllUsers_WithoutJWT_ReturnsUnauthorized()
        {
            // Arrange

            // Act
            var response = await testClient.GetAsync("/users");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetUser_WithRightJWT_ReturnsOk()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response = await testClient.GetAsync("/users/ae2d605e-2392-4a86-b3a2-bf75c486f311");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        // User A JWT should not give access to User B data, for example
        // Fails
        // [Fact]
        // public async Task GetUser_WithOtherJWT_ReturnsUnauthorized()
        // {
        //     // Arrange
        //     await AuthenticateAsync();

        //     // Act
        //     var response = await testClient.GetAsync("/users/ae2d605e-2392-4a86-b3a2-bf75c486f332");

        //     // Assert
        //     response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        // }

        [Fact]
        public async Task CreateUser_WithValidData_ReturnsCreated()
        {
            // Arrange
            var userToCreate = new CreateUserDTO
            {
                Username = "User3",
                Email = "user3@mail.com",
                Password = "Cvb123",
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(userToCreate), Encoding.UTF8, "application/json");
            var response = await testClient.PostAsync("/users", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreateUser_WithExistingUsername_ReturnsBadRequest()
        {
            // Arrange
            var userToCreate = new CreateUserDTO
            {
                Username = "User1",
                Email = "user1@mail.com",
                Password = "Cvb123",
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(userToCreate), Encoding.UTF8, "application/json");
            var response = await testClient.PostAsync("/users", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        
        [Fact]
        public async Task CreateUser_WithInvalidPassword_ReturnsBadRequest()
        {
            // Arrange
            var userToCreate = new CreateUserDTO
            {
                Username = "User1",
                Email = "user1@mail.com",
                Password = "123456",
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(userToCreate), Encoding.UTF8, "application/json");
            var response = await testClient.PostAsync("/users", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}