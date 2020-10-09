using Microsoft.AspNetCore.Authorization;
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
        private readonly IDatabase _redis;
        public IsAdministratorHandler(ScreamDB _db, ConnectionMultiplexer redis)
        {
            this._db = _db;
            _redis = redis.GetDatabase();
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdministratorRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Issuer == AccountAuthorization.Issuer &&
                                        c.Type == ClaimTypes.PrimarySid &&
                                        c.Type == ClaimTypes.Role &&
                                        c.Type == ClaimTypes.Hash))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            //  Validate is administartor
            if (!int.TryParse(context.User.Claims.First(c => c.Type == ClaimTypes.PrimarySid).Value, out int id))
                return Task.CompletedTask;

            if (context.User.Claims.First(c => c.Type == ClaimTypes.Role).Value == Administrator.ROLE_NAME)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
