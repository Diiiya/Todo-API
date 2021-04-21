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


    }
}