using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ScreamBackend.Controllers
{
    [Route("api/client/[controller]")]
    [ApiController]
    public class ScreamAPIBase : ControllerBase
    {
        protected const string ERRORS = "errors";
    }
}
