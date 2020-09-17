﻿using System;
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
        private readonly Screams.IScreamsManager _screamsManager;
        private readonly UserManager<DB.Tables.User> _userManager;
        public ScreamsController(
            Screams.IScreamsManager _screamsManager,
            UserManager<DB.Tables.User> _userManager)
        {
            this._screamsManager = _screamsManager;
            this._userManager = _userManager;
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
            var paging = Screams.ScreamPaging.Create(index, size);
            var result = await _screamsManager.GetScreamsAsync(paging);
            if (result.Successed)
            {
                paging = result.Data;
                return Ok(paging);
            }
            ParseModelStateErrors(result.Errors);
            return BadRequest(ModelState);
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
        public async Task<IActionResult> PostNewScreamAsync([FromBody]Screams.Models.NewScreamtion model)
        {
            if (!int.TryParse(_userManager.GetUserId(User), out int userId))
                return Unauthorized();

            model.ScreamerId = userId;

            var result = await _screamsManager.PostScreamAsync(model);
            if (result.Successed)
                return Created("", result.Data);

            ParseModelStateErrors(result.Errors);
            return BadRequest(ModelState);
        }
    }
}