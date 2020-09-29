using ScreamBackend.DB;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounts
{
    public class AdminManager : IAccountManager<AdminManager>
    {
        private readonly ScreamDB _db;
        private readonly IDatabase _redis;
        public AdminManager(ScreamDB db, ConnectionMultiplexer redisConn)
        {
            _db = db;
            _redis = redisConn.GetDatabase();
        }

        public Task<User> GetUserAsync(ClaimsPrincipal principal)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(string account)
        {
            throw new NotImplementedException();
        }

        public Task<AccountResult> RegisterAsync(Models.RegisterInfo register)
        {
            throw new NotImplementedException();
        }

        public Task<User> SignInAsync(Models.SignInInfo model)
        {
            throw new NotImplementedException();
        }

        private Administrator ReturnAdmimistartor(ScreamBackend.DB.Tables.User userModel)
        {
            return userModel == null ? null : new Administrator(userModel, _db);
        }
    }
}
