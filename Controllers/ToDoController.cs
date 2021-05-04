using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Models;
using System.Threading.Tasks;
using TodoApi.Data.EfCore;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using ToDoAPI.Extensions;
using ToDoAPI.DTOs;
using System.Text.Json;
using ToDoAPI.Models;
using TodoApi.Repositories;

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

        // [Authorize]
        // [HttpGet]
        // public async Task<IEnumerable<ToDoDTO>> GetAllToDosForUserAsync(Guid userId)
        // {
        //     var allToDos = (await repository.GetAll()).Select(todo => todo.ToDoAsDTO());
        //     return allToDos;
        // }

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
        public async Task<ActionResult<ToDoDTO>> CreateToDOAsync(CreateToDoDTO toDo)
        {
            ToDo newToDO = new()
            {
                Id = Guid.NewGuid(),
                Description = toDo.Description,
                Date = toDo.Date,
                Time = toDo.Time,
                Location = toDo.Location,
                Done = false,
                Priority = toDo.Priority,
                FkTagId = toDo.FkTagId,
                FkUserId = toDo.FkUserId //how to get userId
            };

            var myCreatedEntity = await repository.Add(newToDO);

            return CreatedAtAction(nameof(GetToDoAsync), new { id = newToDO.Id }, newToDO.ToDoAsDTO());

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
                FkTagId = toDoDTO.FkTagId,
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