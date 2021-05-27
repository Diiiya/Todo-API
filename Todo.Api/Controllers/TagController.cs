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
    [Route("tags")]
    public class TagController : ControllerBase
    {
        private readonly ITagRepo tagRepo;

        public TagController(ITagRepo tagRepo)
        {
            this.tagRepo = tagRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<TagDTO>> GetAllTagsAsync()
        {
            var allTags = (await tagRepo.GetAll()).Select(tag => tag.TagAsDTO());
            return allTags;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TagDTO>> GetTagAsync(Guid id)
        {
            var tag = await tagRepo.Get(id);

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
                TagColor = tag.TagColor,
                FkUserId = tag.FkUserId
            };

            var myCreatedEntity = await tagRepo.Add(newTag);

            return CreatedAtAction(nameof(GetTagAsync), new { id = newTag.Id }, newTag.TagAsDTO());

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTagAsync(Guid id, CreateTagDTO tag)
        {
            Tag existingTag = await tagRepo.Get(id);

            if (existingTag is null)
            {
                return NotFound();
            }

            Tag updatedTag = existingTag with
            {
                Id = existingTag.Id,
                TagName = tag.TagName,
                TagColor = tag.TagColor,
                FkUserId = existingTag.FkUserId
            };

            await tagRepo.Update(updatedTag);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTagAsync(Guid id)
        {
            Tag existingTag = await tagRepo.Get(id);

            if (existingTag is null)
            {
                return NotFound();
            }

            await tagRepo.Delete(id);

            return NoContent();
        }
    }
}