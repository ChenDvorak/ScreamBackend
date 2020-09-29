using ScreamBackend.DB;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounts
{
    public class Administrator : AbstractUser
    {
        public Administrator(ScreamBackend.DB.Tables.User model, ScreamDB db) : base(model, db)
        { }

        public override async Task<Claim[]> GenerateClaimsAsync()
        {
            Model.Token = Guid.NewGuid().ToString();
            await Update();

            var claims = new[]
            {
                new Claim(ClaimTypes.PrimarySid, Model.Id.ToString()),
                new Claim(ClaimTypes.Role, nameof(Administrator)),
                new Claim(ClaimTypes.Hash, Model.Token)
            };
            return claims;
        }

    }
}
