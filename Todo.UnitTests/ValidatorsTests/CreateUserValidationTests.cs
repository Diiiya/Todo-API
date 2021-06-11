using FluentAssertions;
using Todo.Api.DTOs;
using Todo.Api.Validators;
using Xunit;

namespace Todo.UnitTests.ValidatorsTests
{
    public class CreateUserValidationTests 
    {
        private readonly CreateUserValidator validator = new();

        [Theory]
        [InlineData("")]
        [InlineData("aa")]
        [InlineData("aaa!")]
        // [InlineData("?aaa")] -- Fails and shouldn't be valid
        // [InlineData("a-a")] -- Fails and shouldn't be valid
        [InlineData("aaaź")] 
        [InlineData("кафе")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void ValidateIncorrectUsername_ReturnsFalse(string username)
        {
            // Act
            var userToCreate = new CreateUserDTO()
            {
                Username = username,
                Email = "lalal@mail.com",
                Password = "lala123"
            };

            // Assert
            validator.Validate(userToCreate).IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("aaa")]
        [InlineData("aa1")]
        [InlineData("123")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        public void ValidateCorrectUsername_ReturnsTrue(string username)
        {
            // Act
            var userToCreate = new CreateUserDTO()
            {
                Username = username,
                Email = "lalal@mail.com",
                Password = "lala123"
            };

            // Assert
            validator.Validate(userToCreate).IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("a@y.z")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@.mail.com")]
        [InlineData("amail.com")]
        // [InlineData("aa@mailcom")] -- should it pass?
        // [InlineData("aa@mail.4")] -- should it pass?
        public void ValidateIncorrectEmail_ReturnsFalse(string email)
        {
            // Act
            var userToCreate = new CreateUserDTO()
            {
                Username = "username",
                Email = email,
                Password = "lala123"
            };

            // Assert
            validator.Validate(userToCreate).IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("aa@a.a")]
        [InlineData("123@a.a")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@.aaa")]
        [InlineData("aaaź@mail.fr")] 
        [InlineData("1@abv.bg")]
        public void ValidateCorrectEmail_ReturnsTrue(string email)
        {
            // Act
            var userToCreate = new CreateUserDTO()
            {
                Username = "username",
                Email = email,
                Password = "lala123"
            };

            // Assert
            validator.Validate(userToCreate).IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("aaaa1")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa1")]
        [InlineData("123456")]
        [InlineData("aaaaaa")]
        [InlineData("aaaaa!")]
        [InlineData("кафене")]
        
        public void ValidateIncorrectPassword_ReturnsFalse(string password)
        {
            // Act
            var userToCreate = new CreateUserDTO()
            {
                Username = "username",
                Email =  "lalal@mail.com",
                Password = password
            };

            // Assert
            validator.Validate(userToCreate).IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa1")]
        [InlineData("12345a")]
        [InlineData("1aaaaa")]
        // [InlineData("aaaaa!1")] -- should pass
        // [InlineData("кафене5a")] -- should pass
        
        public void ValidateCorrectPassword_ReturnsTrue(string password)
        {
            // Act
            var userToCreate = new CreateUserDTO()
            {
                Username = "username",
                Email =  "lalal@mail.com",
                Password = password
            };

            // Assert
            validator.Validate(userToCreate).IsValid.Should().BeTrue();
        }
    }
}