namespace RiskAssessment.Models.Data
{
    public class ChangePassword
    {
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
