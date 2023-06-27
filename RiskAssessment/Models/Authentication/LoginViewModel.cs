using System.ComponentModel.DataAnnotations;

namespace RiskAssessment.Models.Authentication
{
    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
