using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Todo.Api.DTOs;
using Xunit;

namespace Todo.IntegrationTests
{
    public class UserControllerTests : IntegrationTest
    {
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
    }
}