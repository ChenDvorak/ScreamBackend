using Microsoft.EntityFrameworkCore;
using ScreamBackend.DB;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Accounts
{
    public class UserManager : AbstractManager<UserManager>
    {
        public UserManager(ScreamDB db, ConnectionMultiplexer redis) : base(db, redis)
        { }

        public Task SignOutAsync(AbstractUser user)
        {
            return user.SignOutAsync();
        }


        protected override AbstractUser ReturnUser(ScreamBackend.DB.Tables.User user)
        {
            return user == null ? null : new Client(user, _db);
        }
    }
}
