using RiskAssessment.Models;
using System.Collections.Generic;

namespace RiskAssessment.Repositorys.IRepositorys
{
    public interface IUserAccountRepository 
    {
        public List<UserModel> getUsers();
        public void insertUser(UserModel user);
        public void updateUser(UserModel user);
        public void updateStatusUser(UserModel user);
        public void UpdatePassword(UserModel user);
    }
}
