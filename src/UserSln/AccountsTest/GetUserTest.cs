using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Accounts;
using Xunit;

namespace AccountsTest
{
    public class GetUserTest : DBSeedFactory
    {

        [Fact]
        public async void GetUser_Account_ReturnUser()
        {
            //  arrange

            IAccountManager<UserManager> accountManager = new UserManager(_db, redisConn);

            //  act
            var account = await accountManager.GetUserAsync(FakeUser.Email);

            //  assert
            Assert.NotNull(account);
        }

        [Fact]
        public async void GetUser_Claims_ReturnUser()
        {
            //  arrange
            var claimsPrincipal = GetClaimsPrincipal();

            IAccountManager<UserManager> accountManager = new UserManager(_db, redisConn);

            //  act
            var account = await accountManager.GetUserAsync(claimsPrincipal);

            //  assert
            Assert.NotNull(account);
        }
        private ClaimsPrincipal GetClaimsPrincipal()
        {
            Claim[] claims = new Claim[] { new Claim(ClaimTypes.PrimarySid, FakeUser.Id.ToString()) };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            principal.Claims.Append(new Claim(ClaimTypes.PrimarySid, FakeUser.Id.ToString()));
            return principal;
        }

        [Fact]
        public async void GetUser_WrongAccount_ReturnNull()
        {
            //  arrange
            const string WRONG_ACCOUNT = "TEST: WRONG_ACCOUNT";
            IAccountManager<UserManager> accountManager = new UserManager(_db, redisConn);

            //  act
            var account = await accountManager.GetUserAsync(WRONG_ACCOUNT);

            //  assert
            Assert.Null(account);
        }
    }
}
