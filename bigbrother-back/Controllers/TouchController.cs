using bigbrother_back.DataContext;
using bigbrother_back.Models.Api;
using bigbrother_back.Models.DataModel;
using bigbrother_back.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using System.Security.Claims;

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

            var account = await DataModel.Accounts.Include(a => a.Marker)
                                                  .Where(a => a.Marker != null)
                                                  .FirstOrDefaultAsync(a => a.Marker!.Id == marker.Id);

            if (account == null)
            {
                return Ok();
            }

            marker.Place = place;

            var visit = new Visit()
            {
                AccountId = account.Id,
                PlaceId = place.Id,
                VisitDate = DateTime.UtcNow,
            };
            DataModel.Visits.Add(visit);

            await DataModel.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// IoT touch action. Place's IoT or manager sends this PUT request when Marker is release any Place position.
        /// </summary>
        [HttpPut("OutPlace")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult> OutPlaceAsync(TouchOutPlaceRequest outPlaceRequest)
        {
            var marker = await DataModel.Markers.FirstOrDefaultAsync(m => m.Signature == outPlaceRequest.MarkerSignature);
            if (marker == null)
            {
                return Problem("Marker not found.", null, StatusCodes.Status404NotFound);
            }

            marker.Place = null;
            await DataModel.SaveChangesAsync();

            return Ok();
        }


        /// <summary>
        /// Customer's "my place" request. Custome sends this GET request when whant to know own placement.
        /// </summary>
        [HttpGet("MyPlace")]
        [Authorize]
        public async Task<ActionResult<MyPlaceResponce>> MyPlaceAsync()
        {
            var user = HttpContext.User;
            var claimAccountId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var account = await DataModel.Accounts.Include(a => a.Marker)
                                                  .FirstOrDefaultAsync(a => a.Id == claimAccountId);
            if (account == null)
            {
                return Problem("Account not found.", null, StatusCodes.Status404NotFound);
            }

            var myPlace = account.Marker?.Place;
            if (myPlace != null)
            {
                var res = new MyPlaceResponce() 
                {
                    Id = myPlace.Id,
                    Name = myPlace.Name,
                    Description = myPlace.Description,
                };

                return Ok(res);
            }

            return Ok();
        }


        /// <summary>
        /// Customer's "recomendations to me" request. Custome sends this GET request when whant to know own placement.
        /// </summary>
        [HttpGet("MyRecomendations")]
        [Authorize]
        public async Task<ActionResult<MyPlaceResponce>> MyRecomendationsAsync()
        {
            var user = HttpContext.User;
            var claimAccountId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var account = await DataModel.Accounts.Include(a => a.Tags)
                                                  .FirstOrDefaultAsync(a => a.Id == claimAccountId);
            if (account == null)
            {
                return Problem("Account not found.", null, StatusCodes.Status404NotFound);
            }

            if (account.Tags == null)
            {
                return Ok();
            }

            /*var visitedPlaces = await DataModel.Places.Where(p => p.Tags != null)
                                                      .Join(account.Tags,);

            var myPlace = account.Marker?.Place;
            if (myPlace != null)
            {
                var res = new MyPlaceResponce()
                {
                    Id = myPlace.Id,
                    Name = myPlace.Name,
                    Description = myPlace.Description,
                };

                return Ok(res);
            }*/

            return Ok();
        }

        #endregion
    }
}
