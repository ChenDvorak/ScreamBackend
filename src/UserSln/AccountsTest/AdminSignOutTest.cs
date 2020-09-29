using Accounts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AccountsTest
{
    public class AdminSignOutTest : DBSeedFactory
    {
        public AdminSignOutTest() : base()
        {
            CreateAdministartor();
        }

        [Fact]
        public async void SignOut_SignInInput_ReturnEmptyToken()
        {
            //  arrange
            const string EXPECT_TOKEN = "";
            Models.SignInInfo info = new Models.SignInInfo
            {
                Account = FakeAdmin.Email,
                Password = FakeAdmin.PasswordHash
            };

            IAccountManager<UserManager> accountManager = new UserManager(_db, redisConn);
            string actualToken;

            //  act
            var user = await accountManager.SignInAsync(info);
            await user.SignOutAsync();
            actualToken = (await _db.Users.AsNoTracking().SingleAsync(u => u.NormalizedEmail == FakeUser.NormalizedEmail)).Token;

            //  assert
            Assert.Equal(EXPECT_TOKEN, actualToken);
        }
    }
}
