using Microsoft.EntityFrameworkCore;
using ScreamBackend.DB;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Accounts
{
    public class UserManager : IAccountManager<UserManager>
    {
        private readonly ScreamDB _db;
        private readonly IDatabase _redis;
        public UserManager(ScreamDB db, ConnectionMultiplexer redis)
        {
            _db = db;
            _redis = redis.GetDatabase();
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <returns></returns>
        public async Task<AccountResult> RegisterAsync(Models.RegisterInfo register)
        {
            if (register.Password != register.ConfirmPassword)
                return AccountResult.Unsuccessful("两次密码不一致");

            var user = await GetUserFromEmailAsync(register.Email);
            if (user != null)
                return AccountResult.Unsuccessful("");

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

        private async Task<ScreamBackend.DB.Tables.User> GetUserFromEmailAsync(string email)
        {
            var model = await _db.Users.AsNoTracking()
                .SingleOrDefaultAsync(u => u.NormalizedEmail == Formator.EmailNormaliz(email));
            return model;
        }

        public Task<AccountResult> AdminSignInAsync(Models.SignInInfo model)
        {
            throw new NotImplementedException();
        }

        public Task SignOutAsync(User user)
        {
            return user.SignOutAsync();
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal principal)
        {
            var claim = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.PrimarySid);
            if (claim == null || !int.TryParse(claim.Value, out int userId))
                return null;

            string key = User.GetCacheKey(userId);

            ScreamBackend.DB.Tables.User model = new ScreamBackend.DB.Tables.User();

            if (!int.TryParse(await _redis.HashGetAsync(key, nameof(ScreamBackend.DB.Tables.User.Id)), out userId))
                model.Id = userId;

            return await GetUserFromIdAsync(userId);
        }

        public async Task<User> GetUserFromIdAsync(int id)
        {
            var model = await _db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
            return new User(model, _db);
        }

        public async Task<User> GetUserAsync(string account)
        {
            if (string.IsNullOrWhiteSpace(account))
                return null;

            var user = await _db.Users.AsNoTracking()
                .Where(u => u.Username.Equals(account, StringComparison.OrdinalIgnoreCase)
                        || u.Email.Equals(account, StringComparison.OrdinalIgnoreCase))
                .SingleOrDefaultAsync();
            return new User(user, _db);
        }

    }
}
