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
            TagDTO myTag = new();
            if(todo.Tag != null){
                myTag = todo.Tag.TagAsDTO();
            }
            return new ToDoDTO
            {
                Id = todo.Id,
                Description = todo.Description,
                DateTime = todo.DateTime,
                Location = todo.Location,
                Priority = todo.Priority,
                FkTagId = todo.FkTagId,
                FkUserId = todo.FkUserId,
                Tag = myTag
            };
        }

        public static TagDTO TagAsDTO(this Tag tag)
        {
            return new TagDTO
            {
                Id = tag.Id,
                TagName = tag.TagName,
                TagColor = tag.TagColor,
                FkUserId = tag.FkUserId
            };
        }
    }
}