using bigbrother_back.DataContext;
using bigbrother_back.Models.Api;
using bigbrother_back.Models.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net.Mime;

namespace bigbrother_back.Controllers
{
    public class TagController : BaseDataContextController
    {
        #region Construction

        public TagController(DatabaseContext dbContext, ILogger<DebugController> logger)
            : base(dbContext, logger)
        {
        }

        #endregion

        #region Endpoints

        [HttpGet("List")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TagResponce>>> GetListAsync([Range(1, MaxPageCount)] int? limit,
                                                                               [Range(0, int.MaxValue)] int? offset)
        {
            var places = await DataModel.Tags.OrderBy(t => t.Id)
                                             .Skip(offset ?? 0)
                                             .Take(limit ?? MaxPageCount)
                                             .ToListAsync();

            var res = places.Select(t => new TagResponce()
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Sex = t.Sex,
                AgeRange = t.AgeRange,
            });

            return Ok(res);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<TagResponce>> GetAsync([Range(1, int.MaxValue)] int id)
        {
            var tag = await DataModel.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (tag == null)
            {
                return Problem("Tag not found", null, StatusCodes.Status404NotFound);
            }

            var res = new TagResponce()
            {
                Id = tag.Id,
                Name = tag.Name,
                Description = tag.Description,
                Sex = tag.Sex,
                AgeRange = tag.AgeRange,
            };

            return Ok(res);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{nameof(AccountRole.Administrator)}, {nameof(AccountRole.Manager)}")]
        public async Task<ActionResult> DeleteAsync([Range(1, int.MaxValue)] int id)
        {
            var tag = await DataModel.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (tag == null)
            {
                return Problem("Tag not found", null, StatusCodes.Status404NotFound);
            }

            DataModel.Tags.Remove(tag);
            await DataModel.SaveChangesAsync();

            return Ok();
        }

        [HttpPost()]
        [Consumes(MediaTypeNames.Application.Json)]
        [Authorize(Roles = $"{nameof(AccountRole.Administrator)}, {nameof(AccountRole.Manager)}")]
        public async Task<ActionResult> CreateAsync(CreateTagRequest tag)
        {
            switch (tag.TagType)
            {
                case TagType.Desciption:
                    if (tag.Sex.HasValue ||
                        tag.AgeRange != null)
                    {
                        ModelState.AddModelError(nameof(tag.TagType), "Description tag should not contain social properties");
                        return ValidationProblem();
                    }
                    break;

                case TagType.Social:
                    if (tag.Sex.HasValue &&
                        tag.AgeRange != null)
                    {
                        ModelState.AddModelError(nameof(tag.TagType), "Social tag should contain social properties");
                        return ValidationProblem();
                    }
                    break;
            }

            DataModel.Tags.Add(new Tag() 
            {
                TagType = tag.TagType,
                Name = tag.Name,
                Description = tag.Description,
                Sex = tag.Sex,
                AgeRange = tag.AgeRange,
            });
            await DataModel.SaveChangesAsync();

            return Ok();
        }

        [HttpPut()]
        [Consumes(MediaTypeNames.Application.Json)]
        [Authorize(Roles = $"{nameof(AccountRole.Administrator)}, {nameof(AccountRole.Manager)}")]
        public async Task<ActionResult> EditAsync(EditTagRequest tag)
        {
            switch (tag.TagType)
            {
                case TagType.Desciption:
                    if (tag.Sex.HasValue ||
                        tag.AgeRange != null)
                    {
                        ModelState.AddModelError(nameof(tag.TagType), "Description tag should not contain social properties");
                        return ValidationProblem();
                    }
                    break;

                case TagType.Social:
                    if (tag.Sex.HasValue &&
                        tag.AgeRange != null)
                    {
                        ModelState.AddModelError(nameof(tag.TagType), "Social tag should contain social properties");
                        return ValidationProblem();
                    }
                    break;
            }

            var currentTag = await DataModel.Tags.FirstOrDefaultAsync(t => t.Id == tag.Id);
            if (currentTag == null)
            {
                return Problem("Tag not found", null, StatusCodes.Status404NotFound);
            }

            currentTag.TagType = tag.TagType;
            currentTag.Name = tag.Name;
            currentTag.Description = tag.Description;
            currentTag.Sex = tag.Sex;
            currentTag.AgeRange = tag.AgeRange;

            await DataModel.SaveChangesAsync();

            return Ok();
        }

        #endregion
    }
}
