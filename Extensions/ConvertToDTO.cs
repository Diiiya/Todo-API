using System;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi
{
    public static class ConvertToDTO
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

        public static ToDoDTO ToDoAsDTO(this ToDo todo)
        {
            return new ToDoDTO
            {
                Id = todo.Id,
                Description = todo.Description,
                Date = todo.Date,
                Done = todo.Done
                //rest?
            };
        }

        public static TagDTO TagAsDTO(this Tag tag)
        {
            return new TagDTO
            {
                Id = tag.Id,
                TagName = tag.TagName,
                //rest?
            };
        }
    }
}