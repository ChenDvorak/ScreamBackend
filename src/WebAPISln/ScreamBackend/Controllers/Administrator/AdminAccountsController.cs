using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ScreamBackend.Controllers.Administrator
{
    public class AdminAccountsController : BaseAdministratorController
    {

        /*
         *  administrator log out
         *  PATCH
         *  route: api/administrator/AdminAccounts
         */
        [HttpPatch]
        public void LogOutAsync()
        {

        }
    }
}
