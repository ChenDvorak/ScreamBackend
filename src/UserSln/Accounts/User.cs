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

        public async Task<bool> IsPasswordMatchAsync(Models.SignInInfo model)
        {
            return await _db.Users.AsNoTracking()
                .Where(u => (u.Username.Equals(model.Account, StringComparison.OrdinalIgnoreCase)
                        || u.Email.Equals(model.Account, StringComparison.OrdinalIgnoreCase))
                        && u.PasswordHash.Equals(model.Password, StringComparison.OrdinalIgnoreCase))
                .AnyAsync();
        }

        public Claim[] GenerateClaims()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.PrimarySid, model.Id.ToString()),
                new Claim(ClaimTypes.Name, model.NormalizedUsername),
                new Claim(ClaimTypes.Email, model.NormalizedEmail)
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
