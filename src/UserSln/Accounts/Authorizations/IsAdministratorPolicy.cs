using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accounts.Authorizations
{
    public class IsAdministratorPolicy
    {
        /// <summary>
        /// the policy name of Is Administartor
        /// </summary>
        public const string POLICY = nameof(IsAdministratorPolicy);


    }
}
