using Microsoft.AspNetCore.Mvc;

namespace RiskAssessment.Controllers
{
    public class OrganizationRiskController : Controller
    {
        public const string Name = "OrganizationRisk";
        public const string ActionIndex = nameof(Index);
        public IActionResult Index()
        {
            return View();
        }
    }
}
