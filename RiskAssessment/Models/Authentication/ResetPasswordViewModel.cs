using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;
using System;

namespace RiskAssessment.Models.Authentication
{
    public class ResetPasswordViewModel
    {
        public ResetPasswordViewModel()
        {

        }

        public ResetPasswordViewModel(UserModel user)
        {
            UserId = user.UserId;
            Username = user.Username;
            ExpireTime = DateTime.Now.AddMinutes(5);
        }
        [Required]
        public int? UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public DateTime? ExpireTime { get; set; }
        public string EmployeeNo { get; set; }
        public bool isFirstLogIn { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public string EncodeModel()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this)));
        }

        public ResetPasswordViewModel DecodeModel(string encodeText)
        {
            try
            {
                return (ResetPasswordViewModel)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(Convert.FromBase64String(encodeText)), typeof(ResetPasswordViewModel));
            }
            catch
            {
                return null;
            }
        }
    }
}
