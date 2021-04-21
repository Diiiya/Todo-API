using System;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi
{
    public static class Extensions
    {
        public static UserDTO AsDTO(this User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Password = user.Password
            };
        }
    }
}