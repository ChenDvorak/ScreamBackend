using ScreamBackend.DB;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Accounts.Authorizations;

namespace Accounts
{
    public class Administrator : AbstractUser
    {
        /// <summary>
        /// The name of role that is administrator 
        /// </summary>
        public const string ROLE_NAME = nameof(Administrator);
        public Administrator(ScreamBackend.DB.Tables.User model, ScreamDB db) : base(model, db)
        { }

        public override async Task<Claim[]> GenerateClaimsAsync()
        {
            Model.Token = Guid.NewGuid().ToString();
            await Update();

            var claims = new[]
            {
                //  Required
                new Claim(ClaimTypes.PrimarySid, Model.Id.ToString(), null, AccountAuthorization.Issuer),
                //  Required
                new Claim(ClaimTypes.Role, ROLE_NAME, null, AccountAuthorization.Issuer),
                //  Required
                new Claim(ClaimTypes.Hash, Model.Token, null, AccountAuthorization.Issuer)
            };
            return claims;
        }

    }
}
