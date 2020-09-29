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

        public override async Task<AccountResult> RegisterAsync(Models.RegisterInfo register)
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
                PasswordHash = register.Password,
                IsAdmin = true
            };
            user.NormalizedUsername = Formator.UsernameNormaliz(user.Username);

            _db.Users.Add(user);
            int effects = await _db.SaveChangesAsync();
            if (effects == 1)
                return AccountResult.Successful;
            throw new Exception("Register fail");
        }

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

        public override async Task<AbstractUser> GetUserAsync(string account)
        {
            if (string.IsNullOrWhiteSpace(account))
                return null;

            string normalizedEmail = Formator.EmailNormaliz(account);
            string normalizedUsername = Formator.UsernameNormaliz(account);

            var user = await _db.Users.AsNoTracking()
                .Where(u => (u.NormalizedUsername == normalizedUsername
                        || u.NormalizedEmail == normalizedEmail) 
                        && u.IsAdmin)
                .SingleOrDefaultAsync();
            return ReturnUser(user);
        }

        public override Task<AbstractUser> GetUserAsync(ClaimsPrincipal principal)
        {
            return base.GetUserAsync(principal);
        }

        protected override async Task<AbstractUser> GetUserFromIdAsync(int id)
        {
            var model = await _db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id && u.IsAdmin);
            return ReturnUser(model);
        }

        protected override AbstractUser ReturnUser(User user)
        {
            return user == null ? null : new Administrator(user, _db);
        }
    }
}
