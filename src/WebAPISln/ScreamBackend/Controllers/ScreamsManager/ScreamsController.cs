using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ScreamBackend.Controllers.ScreamsManager
{
    /// <summary>
    /// screams manager controller
    /// </summary>
    public class ScreamsController : ScreamAPIBase
    {
        private readonly Screams.Screams.IScreamsManager _screamsManager;
        private readonly Accounts.IAccountManager<DB.Tables.User> _accountManager;
        public ScreamsController(
            Screams.Screams.IScreamsManager _screamsManager,
            Accounts.IAccountManager<DB.Tables.User> _accountManager)
        {
            this._screamsManager = _screamsManager;
            this._accountManager = _accountManager;
        }

        /*
         *  Get screams list
         *  
         *  Method:
         *      GET
         *  Route:
         *      api/client/Screams
         *  return:
         *      200 if successful
         *      400 if unsuccessful
         */
        [HttpGet]
        public async Task<IActionResult> GetScreamsAsync(int index, int size)
        {
            var result = await _screamsManager.GetScreamsAsync(index, size);
            return Ok(result);
        }


        /*
         *  post a new scream
         *  must logged in
         *  
         *  Method:
         *      post
         *  Route:
         *      api/client/Screams
         *  return:
         *      200 if successful
         *      400 if unsuccessful
         *      401 if unauthorized
         */
        [HttpPost, Authorize]
        public async Task<IActionResult> PostNewScreamAsync([FromBody] Screams.Models.NewScreamtion model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userModel = await _accountManager.GetUserAsync(User);
            if (userModel == null)
                return Unauthorized();

            model.AuthorId = userModel.Model.Id;

            var result = await _screamsManager.PostScreamAsync(model);
            if (result.Succeeded)
                return Created("", result.Data);

            ParseModelStateErrors(result.Errors);
            return BadRequest(ModelState);
        }
    }
}
