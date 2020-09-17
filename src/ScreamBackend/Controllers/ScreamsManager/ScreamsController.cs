using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ScreamBackend.Controllers.ScreamsManager
{
    /// <summary>
    /// screams manager controller
    /// </summary>
    public class ScreamsController : ScreamAPIBase
    {
        private readonly Screams.IScreamsManager _screamsManager;
        public ScreamsController(Screams.IScreamsManager _screamsManager)
        {
            this._screamsManager = _screamsManager;
        }

        /*
         *  Get screams list
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
    }
}
