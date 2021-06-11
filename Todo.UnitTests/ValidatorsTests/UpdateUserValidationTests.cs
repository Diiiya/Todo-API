using FluentAssertions;
using Todo.Api.DTOs;
using Todo.Api.Validators;
using Xunit;

namespace Todo.UnitTests.ValidatorsTests
{
    public class UpdateUserValidationTests 
    {
        private readonly UpdateUserValidator validator = new();

        [Theory]
        [InlineData("")]
        [InlineData("a@y.z")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@.mail.com")]
        [InlineData("amail.com")]
        // [InlineData("aa@mailcom")] -- should it pass?
        // [InlineData("aa@mail.4")] -- should it pass?
        public void ValidateIncorrectNewEmail_ReturnsFalse(string email)
        {
            // Act
            var userToUpdate = new UpdateUserDTO()
            {
                Email = email,
                CurrentPassword = "lala123"
            };

            // Assert
            validator.Validate(userToUpdate).IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("aa@a.a")]
        [InlineData("123@a.a")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@.aaa")]
        [InlineData("aaaź@mail.fr")] 
        [InlineData("1@abv.bg")]
        public void ValidateCorrectNewEmail_ReturnsTrue(string email)
        {
            // Act
            var userToUpdate = new UpdateUserDTO()
            {
                Email = email,
                CurrentPassword = "lala123"
            };

            // Assert
            validator.Validate(userToUpdate).IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("aaaa1")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa1")]
        [InlineData("123456")]
        [InlineData("aaaaaa")]
        [InlineData("aaaaa!")]
        [InlineData("кафене")]
        public void ValidateIncorrectNewPassword_ReturnsFalse(string newPassword)
        {
            // Act
            var userToUpdate = new UpdateUserDTO()
            {
                Email = "user@mail.com",
                NewPassword = newPassword,
                CurrentPassword = "lala123"
            };

            // Assert
            validator.Validate(userToUpdate).IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa1")]
        [InlineData("12345a")]
        [InlineData("1aaaaa")]
        // [InlineData("aaaaa!1")] -- should pass
        // [InlineData("кафене5a")] -- should pass
        public void ValidateCorrectNewPassword_ReturnsTrue(string newPassword)
        {
            // Act
            var userToUpdate = new UpdateUserDTO()
            {
                Email = "user@mail.com",
                NewPassword = newPassword,
                CurrentPassword = "lala123"
            };

            // Assert
            validator.Validate(userToUpdate).IsValid.Should().BeTrue();
        }

        [Fact]
        public void ValidateMissingCurrentPassword_ReturnsFalse()
        {
            // Act
            var userToUpdate = new UpdateUserDTO()
            {
                Email = "user@mail.com",
                NewPassword = "iii888"
            };

            // Assert
            validator.Validate(userToUpdate).IsValid.Should().BeFalse();
        }

        [Fact]
        public void ValidateMissingNewPassword_ReturnsTrue()
        {
            // Act
            var userToUpdate = new UpdateUserDTO()
            {
                Email = "user@mail.com",
                CurrentPassword = "lala123"
            };

            // Assert
            validator.Validate(userToUpdate).IsValid.Should().BeTrue();
        }

        [Fact]
        public void ValidateMissingEmail_ReturnsFalse()
        {
            // Act
            var userToUpdate = new UpdateUserDTO()
            {
                CurrentPassword = "lala123",
                NewPassword = "iii888"
            };

            // Assert
            validator.Validate(userToUpdate).IsValid.Should().BeFalse();
        }
    }

}