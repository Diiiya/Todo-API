using Todo.Api.DTOs;
using Todo.Api.Models;

namespace Todo.Api
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
                DateTime = todo.DateTime,
                Location = todo.Location,
                Priority = todo.Priority,
            };
        }

        public static TagDTO TagAsDTO(this Tag tag)
        {
            return new TagDTO
            {
                Id = tag.Id,
                TagName = tag.TagName,
                TagColor = tag.TagColor
            };
        }
    }
}