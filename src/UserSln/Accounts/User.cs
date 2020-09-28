using Microsoft.EntityFrameworkCore;
using ScreamBackend.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounts
{
    public class User
    {
        private readonly ScreamBackend.DB.Tables.User model;
        private readonly ScreamDB _db;

        internal User(ScreamBackend.DB.Tables.User model, ScreamDB db)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        internal static string GetCacheKey(int userId) => "USER_" + userId;

        public bool IsPasswordMatch(string passwordHash)
        {
            return model.PasswordHash.Equals(passwordHash, StringComparison.OrdinalIgnoreCase);
        }

        public Claim[] GenerateClaims()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.PrimarySid, model.Id.ToString())
            };
            return claims;
        }

        public Task SignOutAsync()
        {
            model.Token = "";
            _db.Users.Update(model);
            return  _db.SaveChangesAsync();
        }
    }
}
