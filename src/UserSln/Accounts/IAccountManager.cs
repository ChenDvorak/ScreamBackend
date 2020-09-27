﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Accounts
{
    public interface IAccountManager
    {
        public Task<AccountResult> RegisterAsync(Models.RegisterInfo register);

        public Task<AccountResult> AdminSignInAsync(Models.SignInInfo model);


        public Task<User> GetUserAsync(ClaimsPrincipal principal);

        public Task<User> GetUserAsync(string account);
    }
}
