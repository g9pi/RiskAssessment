using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RiskAssessment.Models.Data;
using RiskAssessment.Service.IService;
using System.Security.Claims;
using System;
using RiskAssessment.Models;
using RiskAssessment.Service;
using static RiskAssessment.Models.Common;
using System.Xml.Linq;

namespace RiskAssessment.Controllers
{
    [Authorize]
    public class StorageController : Controller
    {
        public const string Name = "Storage";

        public const string ActionGetImage = nameof(GetImage);
        public const string ActionFile = nameof(GetFile);
        public const string ActionGetImageProfile = nameof(GetImageProfile);
        public const string ActionSaveImageProfile = nameof(SaveImageProfile);

        private readonly IUserAccountService _userAccountService;
        private readonly IStorageService _storageService;
        private IWebHostEnvironment _hostingEnvironment;
        public StorageController(
            IUserAccountService userAccountService,
            IStorageService storageService,
            IWebHostEnvironment environment)
        {
            _userAccountService = userAccountService;
            _storageService = storageService;
            _hostingEnvironment = environment;
        }

        [HttpGet]
        [Route("images/{name}")]
        public IActionResult GetImage(string name)
        {
            try
            {
                var file = _storageService.GetImage(name, AbsolutePath.UserImage);
                return Ok(file);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
        [HttpGet]
        public IActionResult GetImageProfile()
        {
            try
            {
                var userId = int.Parse(HttpContext.User.FindFirstValue(AccountClaim.UserId));
                var fileName = _userAccountService.GetUserByUserId(userId)?.ImageName;
                return Ok(_storageService.GetImage(fileName, AbsolutePath.UserImage));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult SaveImageProfile(int userId, IFormFile file)
        {
            try
            {
                string fileName = null;
                string old_fileName = null;
                if (userId == 0 || file == null) return Json(new { code = 400, message = "UserId and File are required." });
                if (file != null)
                {
                    fileName = _storageService.SaveImage(file, AbsolutePath.UserImage);     
                    UserModel user = _userAccountService.GetUserByUserId(userId);
                    old_fileName = user.ImageName;
                    user.ImageName = fileName;
                    _userAccountService.SaveUser(user);
                }
                if(old_fileName != null)
                {
                    _storageService.DeleteFile(old_fileName);
                }
                return Json(new { code = 200, message = "Image has been uploaded.",filename = fileName });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, message = ex.Message });
            }
        }


        [HttpGet]
        public IActionResult GetFile(string name)
        {
            try
            {
                var file = _storageService.GetFile(name, AbsolutePath.Flows);
                return Ok(file);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

    }
}
