using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounts
{
    public interface IAccountManager<T> where T : class
    {
        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="register"></param>
        /// <returns>return email in data of AccountResult if successful</returns>
        public Task<AccountResult<string>> RegisterAsync(Models.RegisterInfo register);

        public Task<AbstractUser> SignInAsync(Models.SignInInfo model);

        public Task<AbstractUser> GetUserAsync(ClaimsPrincipal principal);

        public Task<AbstractUser> GetUserAsync(string account);
    }
}
