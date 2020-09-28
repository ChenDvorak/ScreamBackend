using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Accounts;

namespace AccountsTest
{
    public class SignInTest : DBSeedFactory
    {

        [Fact]
        public async void SignIn_CorrectInput_ReturnSucceeded()
        {
            //  arrang

            Models.SignInInfo info = new Models.SignInInfo
            { 
                Account = FakeUser.Email,
                Password = FakeUser.PasswordHash
            };

            IAccountManager<UserManager> accountManager = new UserManager(_db, redisConn);

            //  act
            var user = await accountManager.SignInAsync(info);

            //  assert
            Assert.NotNull(user);
        }

        [Fact]
        public async void SignIn_WrongPassword_ReturnSucceeded()
        {
            //  arrang
            const string WRONG_PASSWORD = "11";
            Models.SignInInfo info = new Models.SignInInfo
            {
                Account = FakeUser.Email,
                Password = WRONG_PASSWORD
            };

            IAccountManager<UserManager> accountManager = new UserManager(_db, redisConn);

            //  act
            var user = await accountManager.SignInAsync(info);

            //  assert
            Assert.Null(user);
        }
    }
}
