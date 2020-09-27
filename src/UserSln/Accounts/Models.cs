using System.ComponentModel.DataAnnotations;

namespace Accounts
{
    public class Models
    {
        /// <summary>
        /// Register information
        /// </summary>
        public struct RegisterInfo
        {
            [Required, EmailAddress]
            public string Email { get; set; }
            [Required]
            public string Password { get; set; }
            [Required]
            public string ConfirmPassword { get; set; }
        }

        public class SignInInfo
        {
            public string Account { get; set; }
            public string Password { get; set; }
        }
    }
}
