using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RiskAssessment.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RiskAssessment.Controllers
{
    public class HomeController : Controller
    {
        public const string Name = "Home";
        private readonly ILogger<HomeController> _logger;

        public const string ActionIndex = nameof(Index);
        public const string ActionContact = nameof(Contact);
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [Authorize]
        public IActionResult Index()
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
        [AllowAnonymous]
        [HttpGet("contact")]
        public IActionResult Contact()
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
       

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
