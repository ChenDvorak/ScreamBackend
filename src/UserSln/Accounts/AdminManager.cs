using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounts
{
    public class AdminManager : IAccountManager<AdminManager>
    {
        public Task<User> GetUserAsync(ClaimsPrincipal principal)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(string account)
        {
            throw new NotImplementedException();
        }

        public Task<AccountResult> RegisterAsync(Models.RegisterInfo register)
        {
            throw new NotImplementedException();
        }

        public Task<User> SignInAsync(Models.SignInInfo model)
        {
            throw new NotImplementedException();
        }
    }
}
