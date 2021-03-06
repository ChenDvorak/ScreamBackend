﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Accounts;
using Microsoft.EntityFrameworkCore;

namespace AccountsTest
{
    public class AdminSignInTest : DBSeedFactory
    {
        public AdminSignInTest() : base()
        {
            CreateAdministartor();
        }

        [Fact]
        public async void SignIn_CorrectInput_ReturnSucceeded()
        {
            //  arrang

            Models.SignInInfo info = new Models.SignInInfo
            {
                Account = FakeAdmin.Email,
                Password = FakeAdmin.PasswordHash
            };

            IAccountManager<AdminManager> accountManager = new AdminManager(_db, redisConn);

            //  act
            var user = await accountManager.SignInAsync(info);

            //  assert
            Assert.NotNull(user);
        }

        [Fact]
        public async void SignIn_Password_ReturnSucceeded()
        {
            //  arrange
            IAccountManager<AdminManager> accountManager = new AdminManager(_db, redisConn);

            //  act
            var user = await accountManager.GetUserAsync(FakeAdmin.Email);
            var isMatch = user.IsPasswordMatch(FakeAdmin.PasswordHash);

            //  assert
            Assert.True(isMatch);
        }

        [Fact]
        public async void SignIn_WrongPassword_ReturnSucceeded()
        {
            //  arrang
            const string WRONG_PASSWORD = "11";
            Models.SignInInfo info = new Models.SignInInfo
            {
                Account = FakeAdmin.Email,
                Password = WRONG_PASSWORD
            };

            IAccountManager<AdminManager> accountManager = new AdminManager(_db, redisConn);

            //  act
            var user = await accountManager.SignInAsync(info);

            //  assert
            Assert.Null(user);
        }

        [Fact]
        public async void GenerateToken_CorrectInput_ReturnToken()
        {
            //  arrang
            Models.SignInInfo info = new Models.SignInInfo
            {
                Account = FakeAdmin.Email,
                Password = FakeAdmin.PasswordHash
            };

            IAccountManager<AdminManager> accountManager = new AdminManager(_db, redisConn);
            ScreamBackend.DB.Tables.User userModel;
            //  act
            var user = await accountManager.SignInAsync(info);
            await user.GenerateClaimsAsync();
            userModel = await _db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.NormalizedEmail == FakeAdmin.NormalizedEmail);

            //  assert
            Assert.NotNull(user);
            Assert.False(string.IsNullOrWhiteSpace(userModel.Token));
        }
    }
}
