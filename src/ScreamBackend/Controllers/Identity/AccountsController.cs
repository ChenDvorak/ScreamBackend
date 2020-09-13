using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ScreamBackend.Controllers.Identity
{
    /*
     *  AccountsController
     *  
     */
    public class AccountsController : ScreamAPIBase
    {
        private readonly DB.ScreamDB _db;
        private readonly UserManager<DB.Tables.User> _userManager;
        public AccountsController(DB.ScreamDB _db, UserManager<DB.Tables.User> _userManager)
        {
            this._db = _db;
            this._userManager = _userManager;
        }

        /*
         *  register account
         *  
         *  post
         *  /api/client/accounts
         */
        [HttpPost]
        public IActionResult RegisterAsync([FromBody]string model)
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                ModelState.AddModelError(ERRORS, "empty");
                return BadRequest(ModelState);
            }
            return Ok();
        }
    }
}
