using bigbrother_back.DataContext;
using bigbrother_back.Models.Api;
using bigbrother_back.Models.DataModel;
using bigbrother_back.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Security.Claims;

namespace bigbrother_back.Controllers
{
    public class LoginController : BaseDataContextController
    {
        #region Construction

        public LoginController(DatabaseContext dbContext, ILogger<DebugController> logger)
            : base(dbContext, logger)
        {
        }

        #endregion

        #region Endpoints

        [HttpPost("Salt")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<SaltResponce>> GetSaltAsync(SaltRequest saltRequest)
        {
            var account = await DataModel.Accounts.FirstOrDefaultAsync(a => a.Login == saltRequest.Login);
            if (account == null)
            {
                return Problem("Acccount is not found.", null, StatusCodes.Status404NotFound);
            }

            var res = new SaltResponce()
            {
                HashType = account.HashType,
                Salt = account.Salt
            };
            return Ok(res);
        }

        [HttpPost("Hash")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<LoginResponce>> LoginByHashAsync(LoginRequest loginRequest)
        {
            var account = await DataModel.Accounts.FirstOrDefaultAsync(a => a.Login == loginRequest.Login);
            if (account == null)
            {
                return Problem("Acccount is not found.", null, StatusCodes.Status404NotFound);
            }

            if (account.Hash != loginRequest.Hash)
            {
                return Problem("Wronh hash.", null, StatusCodes.Status401Unauthorized);
            }

            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtRefresh = Security.BuildRefreshToken(account);
            var jwtAccess = Security.BuildAccessToken(account);

            var res = new LoginResponce()
            {
                Id = account.Id,
                Role = account.Role,
                RefreshToken = jwtHandler.WriteToken(jwtRefresh),
                RefreshValidTo = jwtRefresh.ValidTo,
                AccessToken = jwtHandler.WriteToken(jwtAccess),
                AccessValidTo = jwtAccess.ValidTo,
            };
            return Ok(res);
        }

        [HttpPost("Refresh")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<RefreshResponce>> RefreshTokenAsync(RefreshRequest refsrehRequest)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var claims = default(ClaimsPrincipal);

            try
            {
                var jwtInitialRefresh = default(SecurityToken);
                claims = jwtHandler.ValidateToken(refsrehRequest.RefreshToken, new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = Security.JwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = Security.JwtRefreshAudience,
                    ValidateLifetime = true,
                    IssuerSigningKey = Security.JwtSecurityKey,
                    ValidateIssuerSigningKey = true,
                }, out jwtInitialRefresh);
            }
            catch (Exception /*e*/)
            {
                return Problem("Refresh Token is invalid.", null, StatusCodes.Status400BadRequest);
            }

            var claimAccountId = int.Parse(claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            var account = await DataModel.Accounts.FirstOrDefaultAsync(a => a.Id == claimAccountId);
            if (account == null)
            {
                return Problem("Refresh Token refers to empty account.", null, StatusCodes.Status404NotFound);
            }

            // new token pair
            var jwtRefresh = Security.BuildRefreshToken(account);
            var jwtAccess = Security.BuildAccessToken(account);

            var res = new RefreshResponce()
            {
                RefreshToken = jwtHandler.WriteToken(jwtRefresh),
                RefreshValidTo = jwtRefresh.ValidTo,
                AccessToken = jwtHandler.WriteToken(jwtAccess),
                AccessValidTo = jwtAccess.ValidTo,
            };
            return Ok(res);
        }

        [HttpGet("Me")]
        [Authorize]
        public async Task<ActionResult<MeResponce>> MeAsync()
        {
            var user = HttpContext.User;
            var claimAccountId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var account = await DataModel.Accounts.Include(a => a.Marker)
                                                  .FirstOrDefaultAsync(a => a.Id == claimAccountId);
            if (account == null)
            {
                return Problem("Refresh Token refers to empty account.", null, StatusCodes.Status404NotFound);
            }

            var res = new MeResponce()
            {
                Id = account.Id,
                Login = account.Login,
                Role = account.Role,
                Name = account.Name,
                SecondName = account.SecondName,
                Sex = account.Sex,
                BirthDate = account.BirthDate,
                MarkerId = account.Marker?.Id,
            };
            return Ok(res);
        }

        #endregion

    }
}
