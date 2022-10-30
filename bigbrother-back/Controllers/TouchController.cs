using bigbrother_back.DataContext;
using bigbrother_back.Models.Api;
using bigbrother_back.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace bigbrother_back.Controllers
{

    public class TouchController : BaseDataContextController
    {
        #region Construction

        public TouchController(DatabaseContext dbContext, ILogger<DebugController> logger)
            : base(dbContext, logger)
        {
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// IoT touch action. Place's IoT sends this PUT request when Marker identified by signature is in its Place.
        /// </summary>
        [HttpPut("InPlace")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> InPlaceAsync(TouchInPlaceRequest inPlaceRequest)
        {
            var place = await DataModel.Places.FirstOrDefaultAsync(p => p.Id == inPlaceRequest.PlaceId);
            if (place == null)
            {
                return Problem("Place not found.", null, StatusCodes.Status404NotFound);
            }

            var marker = await DataModel.Markers.FirstOrDefaultAsync(m => m.Signature == inPlaceRequest.MarkerSignature);
            if (marker == null)
            {
                return Problem("Marker not found.", null, StatusCodes.Status404NotFound);
            }

            marker.Place = place;
            await DataModel.SaveChangesAsync();

            return Ok();
        }

        #endregion
    }
}
