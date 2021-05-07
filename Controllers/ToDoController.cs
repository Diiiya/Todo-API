using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Models;
using System.Threading.Tasks;
using TodoApi.Data.EfCore;

namespace TodoApi.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("todos")]
    public class ToDoController : ControllerBase
    {
        private readonly EfCoreToDoRepository repository;

        public ToDoController(EfCoreToDoRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<ToDoDTO>> GetAllToDosAsync()
        {
            var allToDos = (await repository.GetAll()).Select(todo => todo.ToDoAsDTO());
            return allToDos;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoDTO>> GetToDoAsync(Guid id)
        {
            var todo = await repository.Get(id);

            if (todo is null)
            {
                return NotFound();
            }

            return Ok(todo.ToDoAsDTO());
        }

        [HttpPost]
        public async Task<ActionResult<ToDoDTO>> CreateToDoAsync(CreateToDoDTO createToDoDTO)
        {
            ToDo newToDo = new()
            {
                Id = Guid.NewGuid(),
                Description = createToDoDTO.Description,
                Date = createToDoDTO.Date,
                Time = createToDoDTO.Time,
                Location = createToDoDTO.Location,
                Done = false,
                Priority = createToDoDTO.Priority,
                FkTagId = createToDoDTO.FkTagId,
                FkUserId = createToDoDTO.FkUserId
            };

            var myCreatedEntity = await repository.Add(newToDo);

            return CreatedAtAction(nameof(GetToDoAsync), new { id = newToDo.Id }, newToDo.ToDoAsDTO());

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateToDOAsync(Guid id, UpdateToDoDTO toDoDTO)
        {
            ToDo existingToDo = await repository.Get(id);

            if (existingToDo is null)
            {
                return NotFound();
            }

            ToDo updatedToDO = existingToDo with
            {
                Id = existingToDo.Id,
                Description = toDoDTO.Description,
                Date = toDoDTO.Date,
                Time = toDoDTO.Time,
                Location = toDoDTO.Location,
                Done = toDoDTO.Done,
                Priority = toDoDTO.Priority,
                FkUserId = existingToDo.FkUserId
            };

            await repository.Update(updatedToDO);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteToDoAsync(Guid id)
        {
            ToDo existingToDo = await repository.Get(id);

            if (existingToDo is null)
            {
                return NotFound();
            }

            await repository.Delete(id);

            return NoContent();
        }
    }
}