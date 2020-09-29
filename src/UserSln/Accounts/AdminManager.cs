using Microsoft.EntityFrameworkCore;
using ScreamBackend.DB;
using ScreamBackend.DB.Tables;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounts
{
    public class AdminManager : AbstractManager<AdminManager>
    {
        public AdminManager(ScreamDB db, ConnectionMultiplexer redisConn): base(db, redisConn)
        { }


        public override async Task<AbstractUser> SignInAsync(Models.SignInInfo model)
        {
            string normalizedAccount = model.Account.ToUpper();
            var userModel = await _db.Users.AsNoTracking()
                                .Where(u => (u.NormalizedUsername == normalizedAccount || u.NormalizedEmail == normalizedAccount)
                                        && u.PasswordHash == model.Password
                                        && u.IsAdmin)
                                .SingleOrDefaultAsync();
            return ReturnUser(userModel);
        }
        protected override AbstractUser ReturnUser(User user)
        {
            return user == null ? null : new Administrator(user, _db);
        }
    }
}
