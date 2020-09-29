using Microsoft.EntityFrameworkCore;
using ScreamBackend.DB;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounts
{
    public abstract class AbstractManager<T> : IAccountManager<T> where T : class
    {
        protected readonly ScreamDB _db;
        protected readonly IDatabase _redis;
        public AbstractManager(ScreamDB db, ConnectionMultiplexer redis)
        {
            _db = db;
            _redis = redis.GetDatabase();
        }

        public virtual async Task<AbstractUser> GetUserAsync(ClaimsPrincipal principal)
        {
            var claim = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.PrimarySid);
            if (claim == null || !int.TryParse(claim.Value, out int userId))
                return null;

            string key = AbstractUser.GetCacheKey(userId);

            ScreamBackend.DB.Tables.User model = new ScreamBackend.DB.Tables.User();

            return await GetUserFromIdAsync(userId);
        }

        protected virtual async Task<AbstractUser> GetUserFromIdAsync(int id)
        {
            var model = await _db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
            return ReturnUser(model);
        }

        public virtual async Task<AbstractUser> GetUserAsync(string account)
        {
            if (string.IsNullOrWhiteSpace(account))
                return null;

            string normalizedEmail = Formator.EmailNormaliz(account);
            string normalizedUsername = Formator.UsernameNormaliz(account);

            var user = await _db.Users.AsNoTracking()
                .Where(u => u.NormalizedUsername == normalizedUsername
                        || u.NormalizedEmail == normalizedEmail)
                .SingleOrDefaultAsync();
            return ReturnUser(user);
        }
        public virtual async Task<AccountResult> RegisterAsync(Models.RegisterInfo register)
        {
            if (register.Password != register.ConfirmPassword)
                return AccountResult.Unsuccessful("两次密码不一致");

            var user = await GetUserFromEmailAsync(register.Email);
            if (user != null)
                return AccountResult.Unsuccessful("已经被使用的邮箱");

            user = new ScreamBackend.DB.Tables.User
            {
                Email = register.Email,
                NormalizedEmail = Formator.EmailNormaliz(register.Email),
                Username = Formator.SplitUsernameFromEmail(register.Email),
                PasswordHash = register.Password
            };
            user.NormalizedUsername = Formator.UsernameNormaliz(user.Username);

            _db.Users.Add(user);
            int effects = await _db.SaveChangesAsync();
            if (effects == 1)
                return AccountResult.Successful;
            throw new Exception("Register fail");
        }

        protected async Task<ScreamBackend.DB.Tables.User> GetUserFromEmailAsync(string email)
        {
            var model = await _db.Users.AsNoTracking()
                .SingleOrDefaultAsync(u => u.NormalizedEmail == Formator.EmailNormaliz(email));
            return model;
        }

        public virtual async Task<AbstractUser> SignInAsync(Models.SignInInfo model)
        {
            string normalizedAccount = model.Account.ToUpper();
            var userModel = await _db.Users.AsNoTracking()
                                .Where(u => (u.NormalizedUsername == normalizedAccount || u.NormalizedEmail == normalizedAccount)
                                        && u.PasswordHash == model.Password)
                                .SingleOrDefaultAsync();
            return ReturnUser(userModel);
        }
        protected abstract AbstractUser ReturnUser(ScreamBackend.DB.Tables.User user);
    }
}
