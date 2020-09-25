using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounts
{
    public interface IAccountManager
    {
        public Task<AccountResult> RegisterAsync();

        public Task<AccountResult> SignInAsync();

        public Task SignOutAsync();

        public Task<User> GetUserAsync(ClaimsPrincipal principal);
    }
}
