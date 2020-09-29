using ScreamBackend.DB;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounts
{
    public class Administrator : User
    {
        public Administrator(ScreamBackend.DB.Tables.User model, ScreamDB db) : base(model, db)
        {
        }

        public override Task<Claim[]> GenerateClaimsAsync()
        {
            throw new NotImplementedException();
        }

        public override bool IsPasswordMatch(string passwordHash)
        {
            throw new NotImplementedException();
        }
    }
}
