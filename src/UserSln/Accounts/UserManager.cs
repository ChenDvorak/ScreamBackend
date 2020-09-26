using Microsoft.EntityFrameworkCore;
using ScreamBackend.DB;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounts
{
    public class UserManager : IAccountManager
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
        public Task<AccountResult> RegisterAsync(Models.RegisterInfo register)
        {
            

            throw new NotImplementedException();
        }

        private async Task<ScreamBackend.DB.Tables.User> GetUserFromEmailAsync(string email)
        {
            var model = await _db.Users.AsNoTracking()
                .SingleOrDefaultAsync(u => u.NormalizedEmail == Formator.EmailNormaliz(email));
            return model;
        }

        public Task<AccountResult> SignInAsync()
        {
            throw new NotImplementedException();
        }

        public Task SignOutAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(ClaimsPrincipal principal)
        {
            throw new NotImplementedException();
        }
    }
}
