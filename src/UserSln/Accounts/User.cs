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
        public readonly ScreamBackend.DB.Tables.User Model;
        private readonly ScreamDB _db;

        internal User(ScreamBackend.DB.Tables.User model, ScreamDB db)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        internal static string GetCacheKey(int userId) => "USER_" + userId;

        public bool IsPasswordMatch(string passwordHash)
        {
            return Model.PasswordHash.Equals(passwordHash, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// sign in status will change when call this function
        /// </summary>
        public async Task<Claim[]> GenerateClaimsAsync()
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

        public Task SignOutAsync()
        {
            Model.Token = "";
            return Update();
        }

        private Task<int> Update()
        {
            _db.Users.Update(Model);
            return _db.SaveChangesAsync();
        }
    }
}
