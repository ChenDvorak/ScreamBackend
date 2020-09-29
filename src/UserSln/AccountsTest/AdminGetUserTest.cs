using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Accounts;
using Xunit;

namespace AccountsTest
{
    public class AdminGetUserTest : DBSeedFactory
    {
        public AdminGetUserTest(): base()
        {
            CreateAdministartor();
        }

        [Fact]
        public async void GetAdmin_Account_ReturnUser()
        {
            //  arrange

            IAccountManager<AdminManager> accountManager = new AdminManager(_db, redisConn);

            //  act
            AbstractUser account = await accountManager.GetUserAsync(FakeAdmin.Email);

            //  assert
            Assert.NotNull(account);
        }

        [Fact]
        public async void GetAdmin_Claims_ReturnUser()
        {
            //  arrange
            var claimsPrincipal = GetClaimsPrincipal();

            IAccountManager<AdminManager> accountManager = new AdminManager(_db, redisConn);

            //  act
            var account = await accountManager.GetUserAsync(claimsPrincipal);

            //  assert
            Assert.NotNull(account);
        }
        private ClaimsPrincipal GetClaimsPrincipal()
        {
            Claim[] claims = new Claim[] { new Claim(ClaimTypes.PrimarySid, FakeAdmin.Id.ToString()) };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            principal.Claims.Append(new Claim(ClaimTypes.PrimarySid, FakeAdmin.Id.ToString()));
            return principal;
        }

        [Fact]
        public async void GetAdmin_WrongAccount_ReturnNull()
        {
            //  arrange
            const string WRONG_ACCOUNT = "TEST: WRONG_ADMINISTARTOR_ACCOUNT";
            IAccountManager<AdminManager> accountManager = new AdminManager(_db, redisConn);

            //  act
            var account = await accountManager.GetUserAsync(WRONG_ACCOUNT);

            //  assert
            Assert.Null(account);
        }
    }
}
