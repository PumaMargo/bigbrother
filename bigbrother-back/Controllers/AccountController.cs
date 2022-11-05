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
    public class AccountController : BaseDataContextController
    {
        #region Construction

        public AccountController(DatabaseContext dbContext, ILogger<DebugController> logger)
            : base(dbContext, logger)
        {
        }

        #endregion

        #region Endpoints

        [HttpGet("List")]
        [Authorize(Roles = $"{nameof(AccountRole.Administrator)}, {nameof(AccountRole.Manager)}")]
        public async Task<ActionResult<IEnumerable<AccountResponce>>> GetListAsync([Range(1, MaxPageCount)] int? limit,
                                                                                   [Range(0, int.MaxValue)] int? offset)
        {
            var accounts = await DataModel.Accounts.Include(a => a.Marker)
                                                   .Include(a => a.Tags)
                                                   .OrderBy(a => a.Id)
                                                   .Skip(offset ?? 0)
                                                   .Take(limit ?? MaxPageCount)
                                                   .ToListAsync();

            var res = accounts.Select(a => new AccountResponce()
            {
                Id = a.Id,
                Login = a.Login,
                Role = a.Role,
                Name = a.Name,
                SecondName = a.SecondName,
                Sex = a.Sex,
                BirthDate = a.BirthDate,
                MarkerId = a.Marker?.Id,
                TagIds = a.Tags?.Select(t => t.Id).ToList(),
            });

            return Ok(res);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{nameof(AccountRole.Administrator)}, {nameof(AccountRole.Manager)}")]
        public async Task<ActionResult<AccountResponce>> GetAsync([Range(1, int.MaxValue)] int id)
        {
            var account = await DataModel.Accounts.Include(a => a.Marker)
                                                  .Include(a => a.Tags)
                                                  .FirstOrDefaultAsync(a => a.Id == id);
            if (account == null)
            {
                return Problem("Account not found", null, StatusCodes.Status404NotFound);
            }

            var res = new AccountResponce()
            {
                Id = account.Id,
                Login = account.Login,
                Role = account.Role,
                Name = account.Name,
                SecondName = account.SecondName,
                Sex = account.Sex,
                BirthDate = account.BirthDate,
                MarkerId = account.Marker?.Id,
                TagIds = account.Tags?.Select(t => t.Id).ToList(),
            };

            return Ok(res);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{nameof(AccountRole.Administrator)}")]
        public async Task<ActionResult> DeleteAsync([Range(1, int.MaxValue)] int id)
        {
            var account = await DataModel.Accounts.FirstOrDefaultAsync(a => a.Id == id);
            if (account == null)
            {
                return Problem("Account not found", null, StatusCodes.Status404NotFound);
            }

            DataModel.Accounts.Remove(account);
            await DataModel.SaveChangesAsync();

            return Ok();
        }

        [HttpPost()]
        [Consumes(MediaTypeNames.Application.Json)]
        [Authorize(Roles = $"{nameof(AccountRole.Administrator)}")]
        public async Task<ActionResult> CreateAsync(CreateAccountRequest account)
        {
            var exists = await DataModel.Accounts.AnyAsync(a => a.Login == account.Login);
            if (exists)
            {
                return Problem("Account already exists", null, StatusCodes.Status404NotFound);
            }

            DataModel.Accounts.Add(new Account() 
            {
                Login = account.Login,
                Role = account.Role,
                Name = account.Name,
                SecondName = account.SecondName,
                Sex = account.Sex,
                BirthDate = account.BirthDate,
            });
            await DataModel.SaveChangesAsync();

            return Ok();
        }

        [HttpPut()]
        [Consumes(MediaTypeNames.Application.Json)]
        [Authorize(Roles = $"{nameof(AccountRole.Administrator)}")]
        public async Task<ActionResult> EditAsync(EditAccountRequest account)
        {
            var currentAccount = await DataModel.Accounts.Include(a => a.Marker)
                                                         .FirstOrDefaultAsync(a => a.Id == account.Id);
            if (currentAccount == null)
            {
                return Problem("Account not found", null, StatusCodes.Status404NotFound);
            }

            currentAccount.Role = account.Role;
            currentAccount.Name = account.Name;
            currentAccount.SecondName = account.SecondName;
            currentAccount.Sex = account.Sex;
            currentAccount.BirthDate = account.BirthDate;
            await DataModel.SaveChangesAsync();

            return Ok();
        }

        #endregion

    }
}
