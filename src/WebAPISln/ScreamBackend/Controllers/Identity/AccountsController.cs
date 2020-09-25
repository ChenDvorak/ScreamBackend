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
        public AccountsController()
        { 

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
         *  Sign in
         *  
         *  PATCH
         *  /api/client/accounts/
         */
        [HttpPatch]
        public async Task<IActionResult> SignInAsync([FromBody] string encodingModel)
        {
            //if (string.IsNullOrWhiteSpace(encodingModel))
            //{
            //    ModelState.AddModelError(ERRORS, "登录信息不能为空");
            //    return BadRequest(ModelState);
            //}

            //var signInModel = JsonConvert.DeserializeObject<DB.Tables.User>(
            //    Encoding.UTF8.GetString(Convert.FromBase64String(encodingModel)));

            //await _signInManager.SignInAsync(signInModel, true);
            //var successed = await _signInManager.CanSignInAsync(signInModel);

            //if (!successed)
            //{
            //    ModelState.AddModelError(ERRORS, "账号或密码错误");
            //    return BadRequest(ModelState);
            //}

            //var user = await _userManager.FindByNameAsync(signInModel.UserName);
            //string token = await _userManager.GetAuthenticationTokenAsync(user, "", "JWT");
            return Ok();
        }

    }
}
