using System.ComponentModel.DataAnnotations;

namespace PRWebAPI.Models.Authentication.SignUp
{
    public class ResetPassword
    {
        [Required]
        public string Email { get; set; } = null;

        public string Token { get; set; } = null;

        [Required] 
        public string Password { get; set; } = null;

        [Compare("Password", ErrorMessage = "The Passwords and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; } = null;
    }
}
