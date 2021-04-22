using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Repositories;
using System.Threading.Tasks;
using TodoApi.Data.EfCore;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        // In Memory Data Source:
        // private readonly IInMemoryUserRepo repository;
        //
        // public UserController(IInMemoryUserRepo repository)
        // {
        //     this.repository = repository;
        // }

        // MSSQL DB
        private readonly EfCoreUserRepository repository;
        public UserController(EfCoreUserRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var allUsers = (await repository.GetAll()).Select(user => user.AsDTO());
            return allUsers;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserAsync(Guid id)
        {
            var user = await repository.Get(id);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user.AsDTO());
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUserAsync(CreateUserDTO userDTO)
        {
            User newUser = new()
            {
                Id = Guid.NewGuid(),
                Username = userDTO.Username,
                Email = userDTO.Email,
                Password = userDTO.Password,
                CreatedDate = DateTimeOffset.UtcNow,
                Deleted = false
            };

            await repository.Add(newUser);

            return CreatedAtAction(nameof(GetUserAsync), new { id = newUser.Id }, newUser.AsDTO());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUserAsync(Guid id, UpdateUserDTO userDTO)
        {
            User existingUser = await repository.Get(id);

            if (existingUser is null)
            {
                return NotFound();
            }

            User updatedUser = existingUser with
            {
                Id = existingUser.Id,
                Username = existingUser.Username,
                Email = userDTO.Email,
                Password = userDTO.Password,
                CreatedDate = existingUser.CreatedDate,
                Deleted = existingUser.Deleted
            };

            await repository.Update(updatedUser);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserAsync(Guid id)
        {
            User existingUser = await repository.Get(id);

            if (existingUser is null)
            {
                return NotFound();
            }

            await repository.Delete(id);

            return NoContent();
        }
    }
}