using RiskAssessment.Models.Data;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace RiskAssessment.Models
{
    public class UserModel
    {
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Division { get; set; }
        public string Section { get; set; }
        public string Group { get; set; }
        public bool Isactive { get; set; }
        public string ImageName { get; set; }
        public string CreateUser { get; set; }
        public string CreateIp { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyUser { get; set; }
        public string ModifyIp { get; set; }
        public DateTime? ModifyDate { get; set; }

        public List<Claim> ToClaims()
        {
            return new List<Claim> {
                new Claim(AccountClaim.UserId, UserId.ToString()),
                new Claim(AccountClaim.Username, Username ?? ""),
                new Claim(AccountClaim.Email, Email ?? ""),
                new Claim(AccountClaim.Name, Name ?? ""),
                new Claim(AccountClaim.UserGroup, Group ?? ""),
                new Claim(AccountClaim.Division, Division??""),
                new Claim(AccountClaim.Section, Section?? "")
            };
        }
    }
    public class Select2
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }
}
