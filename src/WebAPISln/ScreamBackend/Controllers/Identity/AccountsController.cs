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
using Accounts;
using System.IdentityModel.Tokens.Jwt;

namespace ScreamBackend.Controllers.Identity
{
    /*
     *  AccountsController
     *  
     */
    public class AccountsController : ScreamAPIBase
    {
        private readonly IAccountManager<UserManager> _accountManager;
        public AccountsController(IAccountManager<UserManager> accountManager)
        {
            _accountManager = accountManager;
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
            //if (string.IsNullOrWhiteSpace(encodingModel))
            //{
            //    ModelState.AddModelError(ERRORS, "注册信息不能为空");
            //    return BadRequest(ModelState);
            //}

            //var newUser = JsonConvert.DeserializeObject<DB.Tables.User>(
            //    Encoding.UTF8.GetString(Convert.FromBase64String(encodingModel))
            //);
            //var result = await _userManager.CreateAsync(newUser);
            //if (!result.Succeeded)
            //{
            //    foreach (var error in result.Errors)
            //    {
            //        ModelState.AddModelError(ERRORS, error.Description);
            //    }
            //    return BadRequest(ModelState);
            //}

            return Ok();
        }

        /*
         *  Sign in Account
         *  
         *  PATCH
         *  /api/client/accounts/account
         */
        [HttpPatch("account")]
        public async Task<IActionResult> SignInWithAccountAsync([FromBody] string encodingAccount)
        {
            if (string.IsNullOrWhiteSpace(encodingAccount))
            {
                ModelState.AddModelError(ERRORS, "登录账号不能为空");
                return BadRequest(ModelState);
            }

            var decodingAccount =
                Encoding.UTF8.GetString(Convert.FromBase64String(encodingAccount));

            var user = await _accountManager.GetUserAsync(decodingAccount);

            return user == null ? NotFound() : (IActionResult)Ok();
        }

        /*
         *  Sign in
         *  
         *  PATCH
         *  /api/client/accounts/password
         */
        [HttpPatch("password")]
        public async Task<IActionResult> SignInWithPasswordAsync([FromBody] string encodingModel)
        {
            if (string.IsNullOrWhiteSpace(encodingModel))
            {
                ModelState.AddModelError(ERRORS, "登录密码不能为空");
                return BadRequest(ModelState);
            }

            var decodingModel = JsonConvert.DeserializeObject<Models.SignInInfo>(
                Encoding.UTF8.GetString(Convert.FromBase64String(encodingModel)));
            if (decodingModel == null)
            {
                ModelState.AddModelError(ERRORS, "登录信息有误");
                return BadRequest(ModelState);
            }
            var user = await _accountManager.GetUserAsync(decodingModel.Account);
            if (user == null)
                return NotFound();

            var isMatch = user.IsPasswordMatch(decodingModel.Password);
            if (!isMatch)
            {
                ModelState.AddModelError(ERRORS, "密码错误");
                return BadRequest();
            }

            var claims = user.GenerateClaims();
            var jwtToken = AccountAuthorization.GetJwtSecurityToken(claims);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return Ok(token);

        }
    }
}
