using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Accounts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AccountsTest
{
    public class AdminRegisterTest : DBSeedFactory
    {
        public AdminRegisterTest(): base()
        {
            CreateAdministartor();
        }

        [Fact]
        public async void Register_RightInput_ReturnSucceed()
        {
            //  arrange
            const string EMAIL = "AdminDvorak@outlook.com";
            const string NORMALIZED_EMAIL = "ADMINDVORAK@OUTLOOK.COM";
            const string PASSWORD = "96cae35ce8a9b0244178bf28e4966c2ce1b8385723a96a6b838858cdd6ca0a1e";
            const string USERNAME = "AdminDvorak";
            const string NORMALIZED_USERNAME = "ADMINDVORAK";
            
            Models.RegisterInfo register = new Models.RegisterInfo
            { 
                Email = EMAIL,
                Password = PASSWORD,
                ConfirmPassword = PASSWORD
            };
            IAccountManager<AdminManager> accountManager = new AdminManager(_db, redisConn);
            ScreamBackend.DB.Tables.User actual;
            //  act
            var result = await accountManager.RegisterAsync(register);
            actual = await _db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.NormalizedEmail == NORMALIZED_EMAIL);

            //  assert
            Assert.True(result.Succeeded);
            Assert.NotNull(actual);
            Assert.True(actual.IsAdmin);
            Assert.Equal(EMAIL, actual.Email);
            Assert.Equal(NORMALIZED_EMAIL, actual.NormalizedEmail);
            Assert.Equal(USERNAME, actual.Username);
            Assert.Equal(NORMALIZED_USERNAME, actual.NormalizedUsername);
            Assert.Equal(PASSWORD, actual.PasswordHash);
        }

        [Fact]
        public async void Register_DifferencePassword_ReturnFail()
        {
            //  arrange
            const string EMAIL = "DifferencePassword@outlook.com";
            const string PASSWORD = "0";
            const string DIFFERENCE_CONFIRM_PASSWORD = "1";

            const string ERROR = "两次密码不一致";

            Models.RegisterInfo register = new Models.RegisterInfo
            {
                Email = EMAIL,
                Password = PASSWORD,
                ConfirmPassword = DIFFERENCE_CONFIRM_PASSWORD
            };
            IAccountManager<AdminManager> accountManager = new AdminManager(_db, redisConn);
            
            //  act
            var result = await accountManager.RegisterAsync(register);

            //  assert
            Assert.False(result.Succeeded);
            Assert.NotNull(result.Errors);
            Assert.Equal(ERROR, result.Errors.First());
        }
    }
}
