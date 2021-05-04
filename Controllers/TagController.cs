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
    [Route("tags")]
    public class TagController : ControllerBase
    {
        private readonly EfCoreTagRepository repository;

        public TagController(EfCoreTagRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<TagDTO>> GetAllTagsAsync()
        {
            var allTags = (await repository.GetAll()).Select(tag => tag.TagAsDTO());
            return allTags;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TagDTO>> GetTagAsync(Guid id)
        {
            var tag = await repository.Get(id);

            if (tag is null)
            {
                return NotFound();
            }

            return Ok(tag.TagAsDTO());
        }

        [HttpPost]
        public async Task<ActionResult<TagDTO>> CreateTagAsync(CreateAndUpdateTagDTO tag)
        {
            Tag newTag = new()
            {
                Id = Guid.NewGuid(),
                TagName = tag.TagName,
                TagColor = tag.TagColor
            };

            var myCreatedEntity = await repository.Add(newTag);

            return CreatedAtAction(nameof(GetTagAsync), new { id = newTag.Id }, newTag.TagAsDTO());

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTagAsync(Guid id, CreateAndUpdateTagDTO tag)
        {
            Tag existingTag = await repository.Get(id);

            if (existingTag is null)
            {
                return NotFound();
            }

            Tag updatedTag = existingTag with
            {
                Id = existingTag.Id,
                TagName = tag.TagName,
                TagColor = tag.TagColor
            };

            await repository.Update(updatedTag);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTagAsync(Guid id)
        {
            Tag existingTag = await repository.Get(id);

            if (existingTag is null)
            {
                return NotFound();
            }

            await repository.Delete(id);

            return NoContent();
        }
    }
}