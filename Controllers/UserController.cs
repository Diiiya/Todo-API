using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Models;
using TodoApi.Repositories;

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
        public IEnumerable<UserDTO> GetAllUsers()
        {
            return repo.GetAllUsers().Select(user => user.AsDTO());
        }

        [HttpGet("{id}")]
        public ActionResult<UserDTO> GetUser(Guid id)
        {
            var user = repo.GetUser(id);

            if (user is null)
            {
                return NotFound();
            }

            return user.AsDTO();
        }

        [HttpPost]
        public ActionResult<UserDTO> CreateUser(CreateUserDTO userDTO)
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

            repo.CreateUser(newUser);

            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser.AsDTO());
        }

        [HttpPut("{id}")]
        public ActionResult UpdateUser(Guid id, UpdateUserDTO userDTO)
        {
            User existingUser = repo.GetUser(id);

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

            repo.UpdateUser(updatedUser);

            return NoContent();
        }


    }
}