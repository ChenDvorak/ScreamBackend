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
    public abstract class AbstractUser
    {
        public readonly ScreamBackend.DB.Tables.User Model;
        protected readonly ScreamDB _db;

        internal AbstractUser(ScreamBackend.DB.Tables.User model, ScreamDB db)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        internal static string GetCacheKey(int userId) => "USER_" + userId;

        public virtual bool IsPasswordMatch(string passwordHash)
        {
            return Model.PasswordHash.Equals(passwordHash, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// sign in status will change when call this function
        /// </summary>
        public abstract Task<Claim[]> GenerateClaimsAsync();

        public virtual Task SignOutAsync()
        {
            Model.Token = "";
            return Update();
        }

        protected virtual Task<int> Update()
        {
            _db.Users.Update(Model);
            return _db.SaveChangesAsync();
        }
    }
}
