using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using System;
using RiskAssessment.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using RiskAssessment.Models.Authentication;
using RiskAssessment.Service.IService;
using RiskAssessment.Service;
using Microsoft.AspNetCore.Http;
using System.Security.Policy;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.IO;
using RiskAssessment.Models.Data;

namespace RiskAssessment.Controllers
{
    public class AuthenticationController  : BaseController
    {
        public const string Name = "Authentication";

        private readonly IUserAccountService _userAccountService;
        private readonly IEmailService _emailService;
        private readonly IExternalService _externalService;
        // View
        public const string ActionLogin = nameof(Login);
        public const string ActionLogout = nameof(Logout);
        public const string ActionForgetPassword = nameof(ForgetPassword);
        public const string ActionResetPassword = nameof(ResetPassword);

        public const string ActionChangePassword = nameof(changePassword);

        public AuthenticationController(IExternalService externalService, IUserAccountService userAccountService,IEmailService emailService, IConfiguration configuration) : base(configuration)
        {
            _userAccountService = userAccountService;
            _emailService = emailService;
            _externalService = externalService;
        }
        
        [HttpGet("Login")]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            try
            {
                LoginViewModel model = new LoginViewModel
                {
                    ReturnUrl = returnUrl
                };

                //_loggingService.SaveLogging(_logging(Name, ActionLogin));
                return View(model);
                //return View();
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel(500, ex.ToString()));
            }
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewData["Message"] = "Username and password are required";
                    return View(model);
                }
                UserModel account = _userAccountService.GetUserByLogin(model.Username, model.Password);
                _externalService.UpdateUserStatusFromEmployeeNo();
              
                if (account != null && account.Isactive)
                {
                    List<Claim> claims = account.ToClaims();
                    /// เหตุผลที่ไม่ใช้ ClaimTypes และกำหนด Claims เอง
                    /// https://stackoverflow.com/a/36180854
                    if (model.Password == account.EmployeeNo)
                    {
                        //Set("username", model.Username, 5);
                        return RedirectToAction(ActionResetPassword, Name, new { username = account.Username, isFirstLogIn = true });
                    }
                    await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (account != null && !account.Isactive)
                {
                    ViewData["Message"] = "This account is currently unavailable";
                    return View(model);
                }
                else
                {
                    ViewData["Message"] = "Username or password is not correct";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return View(model);
            }
        }
        [HttpGet("Logout")]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction(ActionLogin, Name);
        }
        [HttpGet]
        [Route("forgetpassword")]
        [AllowAnonymous]
        public IActionResult ForgetPassword()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel(500, ex.ToString()));
            }
        }
        
        [HttpGet]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(string username, bool isFirstLogIn)
        {
            try
            {
                
                ResetPasswordViewModel model = new ResetPasswordViewModel();
                if (isFirstLogIn)
                {
                    UserModel acc = _userAccountService.GetUserByUsername(username);
                    model.UserId = acc.UserId;
                    model.Username = acc.Username;
                    model.EmployeeNo = acc.EmployeeNo;
                    model.isFirstLogIn = true;

                }
                else
                {
                    #region Validation
                    model = new ResetPasswordViewModel().DecodeModel(username);
                    if (model == null)
                    {
                        return View("Error", new ErrorViewModel(400, "The link is invalid."));
                    }
                    if (model.ExpireTime < DateTime.Now)
                    {
                        return View("Error", new ErrorViewModel(400, "The link is expired."));
                    }
                    model.isFirstLogIn = false;
                    #endregion

                }
                // return View(model);
                return View(model);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel(500, ex.ToString()));
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("forgetpassword")]
        public IActionResult ForgetPassword(string email)
        {
            try
            {
                
                var user = _userAccountService.GetUserByEmail(email);
                
                if (user == null || !user.Isactive)
                {
                    return Json(new { code = 400, 
                        message = @"We couldn't find any account associated with the provided email address."
                    });
                }
                string body = null;
                using (StreamReader reader = new StreamReader("wwwroot/assets/forgetPassword.html"))
                {
                    body = reader.ReadToEnd();
                }
                EmailModel model = new EmailModel();
                model.Receivers.Add(email);
                model.Host = _emailHost;
                model.Port = _emailPort;
                model.SenderUser = _emailSenderUser;
                model.SenderName = _emailSenderName;
                model.SenderPassword = _emailSenderPassword;

                ResetPasswordViewModel reset = new ResetPasswordViewModel(user);
                string encodeModel = reset.EncodeModel();
                string url = Url.Action(ActionResetPassword, Name, new { username = encodeModel, isFirstLogIn = false }, HttpContext.Request.Scheme);
                body = body.Replace("{Url}", url);
                model.IsBodyHtml = true;

                model.Subject = "RISK ANALYSIS AND ASSESSMENT : Forgot Password";
                model.Message = body;

                _emailService.SendEmail(model);
         

                return Json(new
                {
                    code = 200,
                    message = "Link has been sent to email :" + email
                });
                
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, message = ex.Message });
            }
        }


        //API
        [HttpPost]
        [AllowAnonymous]
        public IActionResult changePassword(ChangePassword input)
        {
            try
            {              
                _userAccountService.ChangePassword((int)input.UserId, input.NewPassword);
                
                TempData["ChangePassword"] = "Pasword has been changed. please Log In with your new Password.";
                return RedirectToAction(ActionLogin, Name);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel(500, ex.ToString()));
            }
        }
    }  
}
