using RiskAssessment.Service.IService;
using System.Text;
using System;
using RiskAssessment.Repositorys.IRepositorys;
using RiskAssessment.Models;
using System.Collections.Generic;
using System.Linq;
using RiskAssessment.Models.Filters;

namespace RiskAssessment.Service
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        public UserAccountService(IUserAccountRepository userAccountRepository)
        {
            _userAccountRepository = userAccountRepository;
        }
        public List<UserModel> getUser(UserFilterModel filter)
        {
            List<UserModel> users = _userAccountRepository.getUsers();
            if(filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Keyword))
                {
                    List<string> keywords = filter.Keyword.Split(",").Where(m => !string.IsNullOrWhiteSpace(m)).Select(m => m.ToLower().Trim()).ToList();
                    users = users.Where(m =>
                        keywords.Any(keyword =>
                            m.Username != null && m.Username.ToLower().Contains(keyword) ||
                            m.EmployeeNo != null && m.EmployeeNo.ToLower().Contains(keyword) ||
                            m.Email != null && m.Email.ToLower().Contains(keyword) ||
                            m.Name != null && m.Name.ToLower().Contains(keyword)

                        )
                    ).ToList();
                }
                if (filter.Group != null)
                {
                    users = users.Where(m => filter.Group.Contains(m.Group)).ToList();
                }
                if (filter.Division != null)
                {
                    users = users.Where(m => filter.Division.Contains(m.Division)).ToList();
                }
                if (filter.Section != null)
                {
                    users = users.Where(m => filter.Section.Contains(m.Section)).ToList();
                }
                if (filter.Isactive != null)
                {
                    users = users.Where(m => (bool)filter.Isactive == m.Isactive).ToList();
                }
            }
            else
            {
                return users;
            }
            
       
                
            return users;
        }
        public UserModel SaveUser(UserModel user)
        {
            if (!user.UserId.HasValue)
            {
                user.Password = EncodePassword(user.EmployeeNo);
                _userAccountRepository.insertUser(user);
            }
            else
            {
                _userAccountRepository.updateUser(user);
            }

            return GetUserByUsername(user.Username);
        }
        public UserModel GetUserByUserId(int userId)
        {
            UserModel user = null;
            List<UserModel> users = _userAccountRepository.getUsers().Where(m => m.UserId == userId).ToList();
            if (users.Count > 0)
            {
                user = users.First();
            }
            return user;
        }
        public void UpdateAccountStatus(UserModel user)
        {
            _userAccountRepository.updateStatusUser(user);            
        }

        public void ChangePassword(int userId, string unencodePassword)
        {
            UserModel model = _userAccountRepository.getUsers().Where(m => m.UserId == userId).FirstOrDefault();
            model.Password = EncodePassword(unencodePassword);
            _userAccountRepository.UpdatePassword(model);
        }
        public UserModel GetUserByUsername(string username)
        {
            UserModel user = null;
            List<UserModel> users = _userAccountRepository.getUsers().Where(m => m.Username.ToLower() == username.ToLower()).ToList();
            if (users.Count > 0)
            {
                user = users.First();
            }
            return user;
        }
        public UserModel GetUserByEmail(string email)
        {
            UserModel user = null;
            List<UserModel> users = _userAccountRepository.getUsers().Where(m => m.Email.ToLower() == email.ToLower()).ToList();
            if (users.Count > 0)
            {
                user = users.First();
            }
            return user;
        }
        public bool IsUniqueUsername(string keyword, int? userId)
        {
            if (string.IsNullOrWhiteSpace(keyword)) throw new Exception("keyword is required.");
            List<UserModel> users = _userAccountRepository.getUsers()
                .Where(m => m.UserId != userId)
                .Where(m => m.Username != null && m.Username.ToLower() == keyword.Trim().ToLower()
                || m.EmployeeNo != null && m.EmployeeNo.ToLower() == keyword.Trim().ToLower()
                || m.Email != null && m.Email.ToLower() == keyword.Trim().ToLower()
                ).ToList();
            if (users.Count > 0) return false;
            else return true;
        }


        public UserModel GetUserByLogin(string username, string unencodePassword)
        {
            var password = EncodePassword(unencodePassword);
            UserModel user = null;
            List<UserModel> users = _userAccountRepository
                .getUsers()
                .Where(m =>
                m.Username.ToLower() == username.ToLower() ||
                !string.IsNullOrEmpty(m.EmployeeNo) && m.EmployeeNo.ToLower() == username.ToLower() ||
                !string.IsNullOrEmpty(m.Email) && m.Email.ToLower() == username.ToLower())
                .Where(m => m.Password == password)
                .ToList();
            if (users.Count > 0)
            {
                user = users.First();
            }
            return user;
        }

        public string EncodePassword(string unencodePassword)
        {
            if (unencodePassword == null) throw new ArgumentNullException("unencodePassword");

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textBytes = Encoding.UTF8.GetBytes(unencodePassword);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);

                return hash;
            }
        }
    }
}
