using RiskAssessment.Models;
using RiskAssessment.Models.Filters;
using System.Collections.Generic;

namespace RiskAssessment.Service.IService
{
    public interface IUserAccountService
    {
        public List<UserModel> getUser(UserFilterModel filter);
        public UserModel SaveUser(UserModel user);
        public UserModel GetUserByLogin(string username, string unencodePassword);
        public UserModel GetUserByUsername(string username);
        public UserModel GetUserByEmail(string email);
        public UserModel GetUserByUserId(int userId);
        public void UpdateAccountStatus(UserModel user);
        public void ChangePassword(int userId, string unencodePassword);
    }
}
