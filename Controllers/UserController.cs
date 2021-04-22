using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Repositories;
using System.Threading.Tasks;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IInMemoryUserRepo repo;
        public UserController(IInMemoryUserRepo repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var allUsers = (await repo.GetAllUsersAsync()).Select(user => user.AsDTO());
            return allUsers;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserAsync(Guid id)
        {
            var user = await repo.GetUserAsync(id);

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

            await repo.CreateUserAsync(newUser);

            return CreatedAtAction(nameof(GetUserAsync), new { id = newUser.Id }, newUser.AsDTO());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUserAsync(Guid id, UpdateUserDTO userDTO)
        {
            User existingUser = await repo.GetUserAsync(id);

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

            await repo.UpdateUserAsync(updatedUser);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserAsync(Guid id)
        {
            User existingUser = await repo.GetUserAsync(id);

            if (existingUser is null)
            {
                return NotFound();
            }

            await repo.DeleteUserAsync(id);

            return NoContent();
        }
    }
}