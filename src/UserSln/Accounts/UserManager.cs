﻿using Microsoft.EntityFrameworkCore;
using ScreamBackend.DB;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Security.Claims;
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

        public Task<User> GetUserAsync(ClaimsPrincipal principal)
        {
            throw new NotImplementedException();
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
