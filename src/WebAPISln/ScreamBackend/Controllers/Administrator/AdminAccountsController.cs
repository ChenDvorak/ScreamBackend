using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ScreamBackend.Controllers.Administrator
{
    /// <summary>
    /// operations of account of administrator 
    /// </summary>
    public class AdminAccountsController : BaseAdministratorController
    {
        private readonly IAccountManager<AdminManager> _administartor;
        public AdminAccountsController(IAccountManager<AdminManager> _administartor)
        {
            this._administartor = _administartor;
        }

        /*
         *  administrator log out
         *  PATCH
         *  route: api/administrator/AdminAccounts
         */
        [HttpPatch]
        public async Task<IActionResult> LogOutAsync()
        {
            var currentAdmin = _administartor.GetUserAsync(User);
            if (currentAdmin == null)
                return Unauthorized();
            throw new NotImplementedException();
        }
    }
}
