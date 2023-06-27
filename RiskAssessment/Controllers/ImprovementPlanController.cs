using Microsoft.AspNetCore.Mvc;

namespace RiskAssessment.Controllers
{
    public class ImprovementPlanController : Controller
    {
        public const string Name = "ImprovementPlan";
        public const string ActionPlanList = nameof(PlanList);
        public const string ActionPlanDetail = nameof(PlanDetail);
        public const string ActionPlanCreate = nameof(CreatePlan);
        public const string ActionPlanEdit = nameof(EditPlan);
        public IActionResult PlanList()
        {
            return View();
        }
        [Route("ImprovementPlan/Detail/{id?}")]
        public IActionResult PlanDetail(int Id)
        {
            return View();
        }
        [Route("ImprovementPlan/Create")]
        public IActionResult CreatePlan()
        {
            return View("EditPlan");
        }
        [Route("ImprovementPlan/Edit/{id?}")]
        public IActionResult EditPlan(int Id)
        {
            return View();
        }
    }
}
