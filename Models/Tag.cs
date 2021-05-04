using System;
using System.Collections.Generic;

namespace TodoApi.Models
{
    public record Tag
    {
        public Guid Id { get; init; }
        public string TagName { get; init; }
        public string TagColor { get; init; } // should be enum?


        public List<ToDo> ToDoItems { get; init; }
    }
}