using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Authorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ScreamBackend.Controllers.Administrator
{
    /// <summary>
    /// administrator controller most inherit this.
    /// there will validate all request that the user is administrator or not.
    /// use [AllowAnonymous] attribute if do not want to validate
    /// </summary>
    [Authorize(policy: IsAdministratorPolicy.POLICY)]
    public abstract class BaseAdministratorController : ScreamAPIBase
    {

    }
}
