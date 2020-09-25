using ScreamBackend.DB;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounts
{
    public class UserManager
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
        public Task<AccountResult> RegisterAsync()
        {
            throw new NotImplementedException();
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
