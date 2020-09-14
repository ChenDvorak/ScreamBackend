using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft;
using Newtonsoft.Json;

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
        public async Task<IActionResult> RegisterAsync([FromBody] string encodingModel)
        {
            if (string.IsNullOrWhiteSpace(encodingModel))
            {
                ModelState.AddModelError(ERRORS, "empty");
                return BadRequest(ModelState);
            }

            var newUser = JsonConvert.DeserializeObject<DB.Tables.User>(
                Encoding.UTF8.GetString(Convert.FromBase64String(encodingModel))
            );
            var result = await _userManager.CreateAsync(newUser);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
