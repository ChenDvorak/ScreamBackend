using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScreamBackend.DB.Tables
{
    public class User: IdentityUser<int>
    {
        /// <summary>
        /// 是否为管理员
        /// </summary>
        [Required]
        public bool IsAdmin { get; set; } = false;
        [Required]
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
        public string Avatar { get; set; } = "";
    }
}
