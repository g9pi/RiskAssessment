using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RiskAssessment.Models.Data;
using System.Net;
using System.Security.Claims;

namespace RiskAssessment.Controllers
{
    public class BaseController : Controller
    {
        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private static string getIpAddress(string type)
        {
            var addlist = Dns.GetHostEntry(Dns.GetHostName());
            string GetHostName = addlist.HostName.ToString();
            string GetIPV6 = addlist.AddressList[0].ToString();
            string GetIPV4 = addlist.AddressList[1].ToString();
            if (type == "V6")
            {
                return GetIPV6;
            }
            else
            {
                return GetIPV4;
            }
        }

        internal readonly IConfiguration _configuration;
        internal string _accountIp => getIpAddress("V4");

        internal int _accountUserId => int.Parse(HttpContext.User.FindFirstValue(AccountClaim.UserId));
        internal string _accountUsername => HttpContext.User.FindFirstValue(AccountClaim.Username);
        internal string _accountEmail => HttpContext.User.FindFirstValue(AccountClaim.Email);
        internal string _accountName => HttpContext.User.FindFirstValue(AccountClaim.Name);
        internal string _accountUserGroup => HttpContext.User.FindFirstValue(AccountClaim.UserGroup);

        internal string _emailHost => _configuration.GetSection("Email:Host").Value;
        internal int _emailPort => int.Parse(_configuration.GetSection("Email:Port").Value);
        internal string _emailSenderUser => _configuration.GetSection("Email:User").Value;
        internal string _emailSenderName => _configuration.GetSection("Email:Name").Value;
        internal string _emailSenderPassword => _configuration.GetSection("Email:Pass").Value;
    }
}
