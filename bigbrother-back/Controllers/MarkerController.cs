using bigbrother_back.DataContext;
using bigbrother_back.Models.Api;
using bigbrother_back.Models.DataModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace bigbrother_back.Controllers
{
    public class MarkerController : BaseDataContextController
    {
        #region Construction

        public MarkerController(DatabaseContext dbContext, ILogger<DebugController> logger)
            : base(dbContext, logger)
        {
        }

        #endregion

        #region Endpoints

        [HttpGet("List")]
        [Authorize(Roles = $"{nameof(AccountRole.Administrator)}, {nameof(AccountRole.Manager)}")]
        public async Task<ActionResult<IEnumerable<MarkerResponce>>> GetListAsync([Range(1, MaxPageCount)] int? limit,
                                                                                  [Range(0, int.MaxValue)] int? offset)
        {
            var markers = await DataModel.Markers.OrderBy(m => m.Id)
                                                 .Skip(offset ?? 0)
                                                 .Take(limit ?? MaxPageCount)
                                                 .ToListAsync();

            var res = markers.Select(m => new MarkerResponce()
            {
                Id = m.Id,
                PlaceId = m.Place?.Id,
            });

            return Ok(res);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{nameof(AccountRole.Administrator)}, {nameof(AccountRole.Manager)}")]
        public async Task<ActionResult<MarkerResponce>> GetAsync([Range(1, int.MaxValue)] int id)
        {
            var marker = await DataModel.Markers.FirstOrDefaultAsync(m => m.Id == id);
            if (marker==null)
            {
                return Problem("Marker not found", null, StatusCodes.Status404NotFound);
            }

            var res = new MarkerResponce()
            {
                Id = marker.Id,
                PlaceId = marker.Place?.Id,
            };

            return Ok(res);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{nameof(AccountRole.Administrator)}, {nameof(AccountRole.Manager)}")]
        public async Task<ActionResult> DeleteAsync([Range(1, int.MaxValue)] int id)
        {
            var marker = await DataModel.Markers.FirstOrDefaultAsync(m => m.Id == id);
            if (marker == null)
            {
                return Problem("Marker not found", null, StatusCodes.Status404NotFound);
            }

            DataModel.Markers.Remove(marker);
            await DataModel.SaveChangesAsync();

            return Ok();
        }

        [HttpPost()]
        [Consumes(MediaTypeNames.Application.Json)]
        [Authorize(Roles = $"{nameof(AccountRole.Administrator)}, {nameof(AccountRole.Manager)}")]
        public async Task<ActionResult> CreateAsync(CreateMarkerRequest marker)
        {
            var exists = await DataModel.Markers.AnyAsync(m => m.Signature == marker.Signature);
            if (exists)
            {
                return Problem("Marker already exists", null, StatusCodes.Status400BadRequest);
            }

            DataModel.Markers.Add(new Marker() { Signature = marker.Signature });
            await DataModel.SaveChangesAsync();

            return Ok();
        }

        #endregion
    }
}
