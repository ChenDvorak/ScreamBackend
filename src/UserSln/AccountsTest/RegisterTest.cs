using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Accounts;
using Microsoft.EntityFrameworkCore;

namespace AccountsTest
{
    public class RegisterTest : DBSeedFactory
    {

        [Fact]
        public async void Register_RightInput_ReturnSucceed()
        {
            //  arrange
            const string EMAIL = "Dvorak@outlook.com";
            const string NORMALIZED_EMAIL = "DVORAK@OUTLOOK.COM";
            const string PASSWORD = "96cae35ce8a9b0244178bf28e4966c2ce1b8385723a96a6b838858cdd6ca0a1e";
            const string USERNAME = "Dvorak";
            const string NORMALIZED_USERNAME = "DVORAK";
            
            Models.RegisterInfo register = new Models.RegisterInfo
            { 
                Email = EMAIL,
                Password = PASSWORD,
                ConfirmPassword = PASSWORD
            };
            IAccountManager<UserManager> accountManager = new UserManager(_db, redisConn);
            ScreamBackend.DB.Tables.User actual;
            //  act
            var result = await accountManager.RegisterAsync(register);
            actual = await _db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.NormalizedEmail == NORMALIZED_EMAIL);

            //  assert
            Assert.True(result.Succeeded);
            Assert.NotNull(actual);
            Assert.Equal(EMAIL, actual.Email);
            Assert.Equal(NORMALIZED_EMAIL, actual.NormalizedEmail);
            Assert.Equal(USERNAME, actual.Username);
            Assert.Equal(NORMALIZED_USERNAME, actual.NormalizedUsername);
            Assert.Equal(PASSWORD, actual.PasswordHash);
        }
    }
}
