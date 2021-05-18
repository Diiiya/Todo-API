using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.DTOs;
using Todo.Api.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Todo.Api.Extensions;
using System.Text.Json;
using Todo.Api.Interfaces;

namespace Todo.Api.Controllers
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
        // private readonly EfCoreUserRepository repository;
        private readonly IUserRepo userRepo;
        PasswordHasher passwordHasher = new PasswordHasher();

        public IConfiguration _configuration;

        public UserController(IUserRepo userRepo, IConfiguration configuration)
        {
            this.userRepo = userRepo;
            this._configuration = configuration;
        }

        // [Authorize]
        [HttpGet]
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var allUsers = (await userRepo.GetAll()).Select(user => user.AsDTO());
            return allUsers;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserAsync(Guid id)
        {
            var user = await userRepo.Get(id);

            if (user is null)
            {
                return NotFound();
            }

            return user.AsDTO();
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUserAsync(CreateUserDTO userDTO)
        {
            User newUser = new()
            {
                Id = Guid.NewGuid(),
                Username = userDTO.Username,
                Email = userDTO.Email,
                Password = passwordHasher.hashPass(userDTO.Password),
                CreatedDate = DateTimeOffset.UtcNow,
                Deleted = false
            };

            var myCreatedEntity = await userRepo.Add(newUser);

            return CreatedAtAction(nameof(GetUserAsync), new { id = newUser.Id }, newUser.AsDTO());
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUserAsync(Guid id, UpdateUserDTO userDTO)
        {
            User existingUser = await userRepo.Get(id);

            if (existingUser is null)
            {
                return NotFound();
            }

            if (passwordHasher.VerifyPassword(existingUser.Password, userDTO.CurrentPassword) == false) 
            {
                return Unauthorized();
            }

            var passwordValue = "";
            if (userDTO.NewPassword != null) 
            {
                passwordValue = passwordHasher.hashPass(userDTO.NewPassword);
            }
            else 
            {
                passwordValue = existingUser.Password;
            }

            User updatedUser = existingUser with
            {
                Id = existingUser.Id,
                Username = existingUser.Username,
                Email = userDTO.Email,
                Password = passwordValue,
                CreatedDate = existingUser.CreatedDate,
                Deleted = existingUser.Deleted
            };

            await userRepo.Update(updatedUser);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUserAsync(Guid id)
        {
            User existingUser = await userRepo.Get(id);

            if (existingUser is null)
            {
                return NotFound();
            }

            await userRepo.Delete(id);

            return NoContent();
        }

        [Authorize]
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] LoginUserDTO userCredentials)
        {
            IEnumerable<UserDTO> users = (await userRepo.GetAll()).Select(user => user.AsDTO());

            var authenticatedUser = users.FirstOrDefault(u =>
                (u.Username == userCredentials.Login || u.Email == userCredentials.Login) &&
                (passwordHasher.VerifyPassword(u.Password, userCredentials.Password) == true));

            if (authenticatedUser == null)
            {
                return BadRequest("Wrong credentials!");
            }
            Token myObjT = CreateToken(authenticatedUser.Id);
            if (myObjT is null)
                return Unauthorized();
            JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);
            var serialized = JsonSerializer.Serialize(myObjT, _jsonOptions);
            var deserialized = JsonSerializer.Deserialize<Token>(serialized, _jsonOptions);
            return Ok(deserialized);
        }

        private Token CreateToken(Guid userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials =
                        new SigningCredentials(
                            new SymmetricSecurityKey(tokenKey),
                            SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);


            string myToken = tokenHandler.WriteToken(token);
            Token myobjT = new()
            {
                CreatedToken = myToken
            };

            return myobjT;
        }
    }
}