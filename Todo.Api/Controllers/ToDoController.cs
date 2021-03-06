using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.DTOs;
using Todo.Api.Models;
using System.Threading.Tasks;
using Todo.Api.Interfaces;

namespace Todo.Api.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("todos")]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoRepo todoRepo;

        public ToDoController(IToDoRepo todoRepo)
        {
            this.todoRepo = todoRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<ToDoDTO>> GetAllToDosAsync()
        {
            var allToDos = (await todoRepo.GetAll()).Select(todo => todo.ToDoAsDTO());
            return allToDos;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoDTO>> GetToDoAsync(Guid id)
        {
            var todo = await todoRepo.Get(id);

            if (todo is null)
            {
                return NotFound();
            }

            return Ok(todo.ToDoAsDTO());
        }

        [HttpGet("user/{userId}")]
        public async Task<IEnumerable<ToDoDTO>> GetAllToDosByUserAsync(Guid userId)
        {
            var allToDos = (await todoRepo.GetAllTodosByUser(userId)).Select(todo =>
            {
                todo.Tag.TagAsDTO();
                return todo.ToDoAsDTO();
            });
            return allToDos;
        }

        [HttpPost]
        public async Task<ActionResult<ToDoDTO>> CreateToDoAsync(CreateToDoDTO createToDoDTO)
        {
            ToDo newToDo;
            if (createToDoDTO.FkTagId != Guid.Empty)
            {
                newToDo = new()
                {
                    Id = Guid.NewGuid(),
                    Description = createToDoDTO.Description,
                    DateTime = createToDoDTO.DateTime,
                    Location = createToDoDTO.Location,
                    Priority = createToDoDTO.Priority,
                    FkTagId = createToDoDTO.FkTagId,
                    FkUserId = createToDoDTO.FkUserId
                };
            }
            else
            {
                newToDo = new()
                {
                    Id = Guid.NewGuid(),
                    Description = createToDoDTO.Description,
                    DateTime = createToDoDTO.DateTime,
                    Location = createToDoDTO.Location,
                    Priority = createToDoDTO.Priority,
                    FkUserId = createToDoDTO.FkUserId
                };
            }
            
            var myCreatedEntity = await todoRepo.Add(newToDo);

            return CreatedAtAction(nameof(GetToDoAsync), new { id = newToDo.Id }, newToDo.ToDoAsDTO());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateToDOAsync(Guid id, UpdateToDoDTO toDoDTO)
        {
            ToDo existingToDo = await todoRepo.Get(id);

            if (existingToDo is null)
            {
                return NotFound();
            }

            ToDo updatedToDO = existingToDo with
            {
                Id = existingToDo.Id,
                Description = toDoDTO.Description,
                DateTime = toDoDTO.DateTime,
                Location = toDoDTO.Location,
                Priority = toDoDTO.Priority,
                FkUserId = existingToDo.FkUserId
            };

            await todoRepo.Update(updatedToDO);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateToDODoneAsync(Guid id)
        {
            ToDo existingToDo = await todoRepo.Get(id);

            if (existingToDo is null)
            {
                return NotFound();
            }

            ToDo updatedToDO = existingToDo with
            {
                Done = !existingToDo.Done,
            };

            await todoRepo.Update(updatedToDO);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteToDoAsync(Guid id)
        {
            ToDo existingToDo = await todoRepo.Get(id);

            if (existingToDo is null)
            {
                return NotFound();
            }

            await todoRepo.Delete(id);

            return NoContent();
        }
    }
}