using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System;
using RiskAssessment.Models;
using RiskAssessment.Service.IService;
using Microsoft.AspNetCore.Authorization;
using RiskAssessment.Models.Account;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using RiskAssessment.Service;
using Microsoft.Extensions.Configuration;
using RiskAssessment.Models.Filters;
using System.Data;
using System.Security.Claims;
using RiskAssessment.Models.Data;

namespace RiskAssessment.Controllers
{
    [Authorize]
    public class AccountController :  BaseController
    {
        public const string Name = "Account";
        //View
        public const string ActionAccountsList = nameof(AccountsList);
        public const string ActionEditUser = nameof(EditUser);
        public const string ActionProfile = nameof(Profile);
        //public const string ActionCreateUser = nameof(CreateUser);

        //API
        public const string ActionGetEmployeeDDL = nameof(getEmpNameDropdown);
        public const string ActionGetEmployeeByEmpNo = nameof(getEmployeeByEmpNo);
        public const string ActionIsUnique = nameof(isUniqueInput);
        public const string ActionSaveUser = nameof(saveUser);
        public const string ActionForceChangePassword = nameof(ForceChangePassword);
        public const string ActionUpdateStatus = nameof(updateUserStatus);
        public const string ActionChangePassword = nameof(changePassword);
        //Service
        private readonly IUserAccountService _userAccountService;
        private readonly IExternalService _externalService;


        public AccountController(IExternalService externalService, IUserAccountService userAccountService, IConfiguration configuration) : base(configuration)
        {
            _externalService = externalService;
            _userAccountService = userAccountService;
        }

        [Route("settings/accounts")]
        public IActionResult AccountsList(UserFilterModel filter)
        {
            try
            {
                AccountListModel model = null;               
                model = new AccountListModel();
                model = model.ToModel(filter);
                model.Section_ddl = _externalService.getSections();
                model.Users = _userAccountService.getUser(filter);
                
                model.Users = model.ToListModel(model.Users);

                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel(500, ex.Message, false));
            }
        }
        [Route("settings/accounts/{id?}")]
        public IActionResult EditUser(int Id)
        {
            try
            {
                EditUserModel model = new EditUserModel();
                var user = _userAccountService.GetUserByUserId(Id);
                model.User = user;
                model.Section_ddl = _externalService.getSections();
               // model.Users = _userAccountService.getUser(filter);
                return View(model);
            }
            catch(Exception ex)
            {
                return View("Error", new ErrorViewModel(500, ex.Message, false));
            }
        }
        [Route("profile")]
        public IActionResult Profile()
        {
            var userId = _accountUserId;
            var item = _userAccountService.GetUserByUserId(userId);
            EditUserModel model = new EditUserModel();
            model.User = item;
            model.Section_ddl = _externalService.getSections();
            return View(model);
        }
        public IActionResult ProfileImg()
        {
            UserModel imgfile = _userAccountService.GetUserByUserId(int.Parse(HttpContext.User.FindFirstValue(AccountClaim.UserId)));
            string filename = imgfile.ImageName;

            ViewBag.filename = filename;
            return PartialView();
        }


        //API
        [HttpPost]
        public JsonResult saveUser(UserModel User)
        {
            try
            {
                User.CreateUser = _accountUsername;
                User.CreateIp = _accountIp;
                User.ModifyUser = _accountUsername;
                User.ModifyIp = _accountIp;
                UserModel user = _userAccountService.SaveUser(User);
                return Json(true);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, message = ex.Message, detail = ex.ToString() });
            }
        }

        [HttpGet]
        public JsonResult getEmployeeByEmpNo(string emp_no)
        {
            try
            {
                var emp_list = _externalService.getEmployee().Where(e=>e.EmployeeNo == emp_no).FirstOrDefault();

               
                return Json(new { data = emp_list });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, message = ex.Message, detail = ex.ToString() });
            }

        }
        [HttpGet]
        public JsonResult getEmpNameDropdown(string q)
        {
            try
            {
                var emp_list = _externalService.getEmployee();

                List<Select2> list = new List<Select2>();
                foreach (var e in emp_list)
                {
                    list.Add(new Select2 { Id = e.EmployeeNo, Text = (e.EmployeeName + " (" + e.SectionNo + ")") });
                }
                if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
                {
                    list = list.Where(x => x.Text.ToLower().StartsWith(q.ToLower()) || x.Text.ToLower().Contains(q.ToLower())).ToList();
                    //item = item.Where(x => x.Text.ToLower().StartsWith(q.ToLower())).ToList();
                }
                return Json(new { items = list });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, message = ex.Message, detail = ex.ToString() });
            }           
        }

        [HttpPost]
        public JsonResult isUniqueInput(UserModel input)
        {
            try
            {
                bool isuniqueUsername = false;
                bool isuniqueEmail = false;
                bool isuniqueEmpNo = false;

                isuniqueUsername = _userAccountService.GetUserByUsername(input.Username) == null;
                isuniqueEmail = _userAccountService.GetUserByEmail(input.Email) == null;
                isuniqueEmpNo = _userAccountService.getUser(null).Where(e => e.EmployeeNo == input.EmployeeNo).FirstOrDefault() == null; 

                return Json(new { Username  = isuniqueUsername,Email = isuniqueEmail , EmpNo = isuniqueEmpNo });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, message = ex.Message, detail = ex.ToString() });
            }
        }
        [HttpPost]
        public JsonResult updateUserStatus(int userId,bool status)
        {
            try
            {
                UserModel user = new UserModel();
                user.UserId = userId;
                user.Isactive = status;
                user.ModifyUser = _accountUsername;
                user.ModifyIp = _accountIp;
                _userAccountService.UpdateAccountStatus(user);
                return Json(new { code = 200, message = "Status has been updated." });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, message = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult ForceChangePassword(int userId)
        {
            try
            {
                #region Validation
                UserModel user = _userAccountService.GetUserByUserId(userId);
                if (user == null)
                {
                    return Json(new { code = 400, message = $"User ID `{userId}` is not found." });
                }
                //if (string.IsNullOrEmpty(password))
                //{
                //    return Json(new { code = 400, message = $"Password is required." });
                //}
                #endregion

                #region Change Password
                string password = user.EmployeeNo;
                _userAccountService.ChangePassword(userId, password);
                #endregion

                return Json(new { code = 200, message = "Password has been changed." });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult changePassword(ChangePassword input)
        {
            try
            {

                UserModel user = _userAccountService.GetUserByLogin(input.Username,input.OldPassword);
                
                if(user == null)
                {
                    return Json(new { code = 400, message = "Old Password is wrong." });
                }
                user.ModifyUser = _accountUsername;
                user.ModifyIp = _accountIp;
                _userAccountService.ChangePassword((int)user.UserId, input.NewPassword);
                return Json(new { code = 200, message = "Password has been changed." });

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, message = ex.Message });
            }
        }
    }
}
