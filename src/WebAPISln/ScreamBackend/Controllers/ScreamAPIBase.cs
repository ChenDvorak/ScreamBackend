using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ScreamBackend.Controllers
{
    [Route("api/administrator/[controller]")]
    [ApiController]
    public class ScreamAPIBase : ControllerBase
    {
        /// <summary>
        /// there is a key in ModelState.AddModelError()
        /// </summary>
        protected const string ERRORS = "errors";

        /// <summary>
        /// quick to call ModelState.AddModelError() to add range of error
        /// </summary>
        /// <param name="errors"></param>
        protected void ParseModelStateErrors(ICollection<string> errors)
        {
            foreach (string error in errors)
            {
                ModelState.AddModelError(ERRORS, error);
            }
        }
    }
}
