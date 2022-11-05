using bigbrother_back.DataContext;
using bigbrother_back.Models.DataModel;
using bigbrother_back.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

namespace bigbrother_back.Controllers
{
    public class DebugController : BaseDataContextController
    {
        #region Properties

        IWebHostEnvironment Environment { get; init; }

        #endregion

        public DebugController(DatabaseContext dbContext,
                               ILogger<DebugController> logger,
                               IWebHostEnvironment environment)
            : base(dbContext, logger)
        {
            Environment = environment;
        }

        #region Endpoints

        [HttpPost("RecreateDatabase")]
        public async Task<ActionResult> RecreateDatabaseAsync()
        {
            if (!Environment.IsDevelopment())
            {
                return BadRequest();
            }

            await DataModel.RecreateDatabaseAsync();
            return Ok();
        }

        [HttpPost("FillDatabase")]
        public async Task<ActionResult> FillDatabaseAsync()
        {
            if (!Environment.IsDevelopment())
            {
                return BadRequest();
            }

            var tags = new List<Tag>()
            {
                new Tag() { TagType = TagType.Social, Name = "Middle-aged men", Sex = AccountSex.Male, AgeRange = new AgeRange() { From = 30, To = 55 } },
                new Tag() { TagType = TagType.Social, Name = "Young women", Sex = AccountSex.Female, AgeRange = new AgeRange() { From = 18, To = 25 } },
            };

            var accounts = new List<Account>()
            {
                new Account() { Login = "margaret.kyselgova@gmail.com", Role = AccountRole.Administrator, Name = "PumaMargo"},
                new Account() { Login = "kyselgov@gmail.com", Role = AccountRole.Administrator, Name = "Tester" },
            };
            accounts.ForEach(account => account.BuildPasswordHash("bbTest"));

            var markerss = new List<Marker>()
            {
                new Marker() { Signature = "rfid#Test1" },
                new Marker() { Signature = "rfid#Test2" },
            };

            var places = new List<Place>()
            {
                new Place() { Name = "Welcome Place", Tags = new List<Tag>() { tags[0] } },
                new Place() { Name = "Test Place", Tags = new List<Tag>() { tags[1] } },
            };

            await DataModel.Accounts.AddRangeAsync(accounts);
            await DataModel.Markers.AddRangeAsync(markerss);
            await DataModel.Places.AddRangeAsync(places);
            await DataModel.SaveChangesAsync();

            return Ok();
        }

        #endregion
    }
}
