using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ScreamBackend.DB;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Authorizations
{
    public class IsAdministratorHandler : AuthorizationHandler<IsAdministratorRequirement>
    {
        private readonly ScreamDB _db;
        public IsAdministratorHandler(ScreamDB _db)
        {
            this._db = _db;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdministratorRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Issuer == AccountAuthorization.Issuer &&
                                        c.Type == ClaimTypes.PrimarySid &&
                                        c.Type == ClaimTypes.Role &&
                                        c.Type == ClaimTypes.Hash))
            {
                context.Fail();
                return;
            }

            var claims = context.User.Claims;

            //  Validate is administartor
            if (!int.TryParse(claims.First(c => c.Type == ClaimTypes.PrimarySid).Value, out int id))
                return;

            //  Validate token for is signned in
            var token = claims.First(c => c.Type == ClaimTypes.Hash).Value;
            var tokenExist = await CheckTokenOfAdministratorAsync(token);
            if (!tokenExist)
            {
                context.Fail();
                return;
            }

            if (claims.First(c => c.Type == ClaimTypes.Role).Value == Administrator.ROLE_NAME)
                context.Succeed(requirement);

        }

        /// <summary>
        /// check token is variable by token from claim
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<bool> CheckTokenOfAdministratorAsync(string token)
        {
            return await _db.Users.AnyAsync(user => user.IsAdmin && user.Token == token);
        }
    }
}
