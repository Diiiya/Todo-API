using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Models;
using System.Threading.Tasks;
using TodoApi.Data.EfCore;
using ToDoAPI.DTOs;

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
        public async Task<ActionResult<TagDTO>> CreateTagAsync(CreateTagDTO tag)
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
        public async Task<ActionResult> UpdateTagAsync(Guid id, UpdateTagDTO tag)
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