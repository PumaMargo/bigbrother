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
    public class PlaceController : BaseDataContextController
    {
        #region Construction

        public PlaceController(DatabaseContext dbContext, ILogger<DebugController> logger)
            : base(dbContext, logger)
        {
        }

        #endregion

        #region Endpoints

        [HttpGet("List")]
        [Authorize(Roles = $"{nameof(AccountRole.Administrator)}, {nameof(AccountRole.Manager)}")]
        public async Task<ActionResult<IEnumerable<PlaceResponce>>> GetListAsync([Range(1, MaxPageCount)] int? limit,
                                                                                 [Range(0, int.MaxValue)] int? offset)
        {
            var places = await DataModel.Places.OrderBy(p => p.Id)
                                               .Skip(offset ?? 0)
                                               .Take(limit ?? MaxPageCount)
                                               .ToListAsync();

            var res = places.Select(p => new PlaceResponce()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
            });

            return Ok(res);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{nameof(AccountRole.Administrator)}, {nameof(AccountRole.Manager)}")]
        public async Task<ActionResult<PlaceResponce>> GetAsync([Range(1, int.MaxValue)] int id)
        {
            var place = await DataModel.Places.FirstOrDefaultAsync(p => p.Id == id);
            if (place == null)
            {
                return Problem("Place not found", null, StatusCodes.Status404NotFound);
            }

            var res = new PlaceResponce()
            {
                Id = place.Id,
                Name = place.Name,
                Description = place.Description,
            };

            return Ok(res);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{nameof(AccountRole.Administrator)}, {nameof(AccountRole.Manager)}")]
        public async Task<ActionResult> DeleteAsync([Range(1, int.MaxValue)] int id)
        {
            var place = await DataModel.Places.FirstOrDefaultAsync(p => p.Id == id);
            if (place == null)
            {
                return Problem("Marker not found", null, StatusCodes.Status404NotFound);
            }

            DataModel.Places.Remove(place);
            await DataModel.SaveChangesAsync();

            return Ok();
        }

        [HttpPost()]
        [Consumes(MediaTypeNames.Application.Json)]
        [Authorize(Roles = $"{nameof(AccountRole.Administrator)}, {nameof(AccountRole.Manager)}")]
        public async Task<ActionResult> CreateAsync(CreatePlaceRequest place)
        {
            DataModel.Places.Add(new Place() { Name = place.Name, Description = place.Description });
            await DataModel.SaveChangesAsync();

            return Ok();
        }

        #endregion
    }
}
