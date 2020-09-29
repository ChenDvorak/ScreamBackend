using ScreamBackend.DB;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounts
{
    public class Client : AbstractUser
    {
        internal Client(ScreamBackend.DB.Tables.User model, ScreamDB db) : base(model, db)
        { }

        /// <summary>
        /// sign in status will change when call this function
        /// </summary>
        public override async Task<Claim[]> GenerateClaimsAsync()
        {
            Model.Token = Guid.NewGuid().ToString();
            await Update();

            var claims = new[]
            {
                new Claim(ClaimTypes.PrimarySid, Model.Id.ToString()),
                new Claim(ClaimTypes.Hash, Model.Token)
            };
            return claims;
        }
    }
}
