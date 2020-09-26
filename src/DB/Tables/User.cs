using System.ComponentModel.DataAnnotations;

namespace ScreamBackend.DB.Tables
{
    public class User: EntityBase
    {
        [Required, StringLength(32)]
        public string Username { get; set; }
        [Required, StringLength(32)]
        public string NormalizedUsername { get; set; }
        [Required, StringLength(256)]
        public string Email { get; set; }
        [Required, StringLength(256)]
        public string NormalizedEmail { get; set; }
        [Required, StringLength(256)]
        public string PasswordHash { get; set; }
        [Required, StringLength(256)]
        public string Token { get; set; }
        /// <summary>
        /// 是否为管理员
        /// </summary>
        [Required]
        public bool IsAdmin { get; set; } = false;
        public string Avatar { get; set; } = "";
    }
}
