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

        /// <summary>
        /// sign in status will change when call this function
        /// </summary>
        public async Task<Claim[]> GenerateClaimsAsync()
        {
            model.Token = Guid.NewGuid().ToString();
            await Update();

            var claims = new[]
            {
                new Claim(ClaimTypes.PrimarySid, model.Id.ToString()),
                new Claim(ClaimTypes.Hash, model.Token)
            };
            return claims;
        }

        public Task SignOutAsync()
        {
            model.Token = "";
            return Update();
        }

        private Task<int> Update()
        {
            _db.Users.Update(model);
            return _db.SaveChangesAsync();
        }
    }
}
