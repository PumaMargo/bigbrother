﻿using bigbrother_back.DataContext;
using bigbrother_back.Models.DataModel;
using bigbrother_back.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

namespace bigbrother_back.Controllers
{
    public class DebugController : BaseController
    {
        #region Properties

        DatabaseContext DataContext { get; init; }

        ILogger<DebugController> Logger { get; init; }

        IWebHostEnvironment Environment { get; init; }

        #endregion
        
        public DebugController(DatabaseContext dbContext,
                               ILogger<DebugController> logger,
                               IWebHostEnvironment environment)
        {
            DataContext = dbContext;
            Logger = logger;
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

            await DataContext.RecreateDatabaseAsync();
            return Ok();
        }

        [HttpPost("FillDatabase")]
        public async Task<ActionResult> FillDatabaseAsync()
        {
            if (!Environment.IsDevelopment())
            {
                return BadRequest();
            }

            var accounts = new List<Account>()
            {
                new Account() { Login = "margaret.kyselgova@gmail.com", Role = AccountRole.Administrator, Name = "PumaMargo"},
                new Account() { Login = "kyselgov@gmail.com", Role = AccountRole.Administrator, Name = "Tester" },
            };
            accounts.ForEach(account => account.BuildPasswordHash("bbTest"));

            var markerss = new List<Marker>()
            {
                new Marker() { HardwareId = "rfid#Test1" },
                new Marker() { HardwareId = "rfid#Test2" },
            };

            var places = new List<Place>()
            {
                new Place() { Name = "Welcome Place" },
                new Place() { Name = "Test Place" },
            };

            await DataContext.Accounts.AddRangeAsync(accounts);
            await DataContext.Markers.AddRangeAsync(markerss);
            await DataContext.Places.AddRangeAsync(places);
            await DataContext.SaveChangesAsync();

            return Ok();
        }

        #endregion
    }
}
