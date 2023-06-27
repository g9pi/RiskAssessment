using Microsoft.AspNetCore.Mvc;

namespace RiskAssessment.Controllers
{
    public class ProductRiskController : Controller
    {
        public const string Name = "ProductRisk";
        public const string ActionProductRiskList = nameof(ProductRiskList);
        public const string ActionProductRiskDetail = nameof(ProductRiskDetail);
        public const string ActionProductRiskCreate = nameof(ProductRiskCreate);
        public const string ActionProductRiskEdit = nameof(ProductRiskEdit);
        public IActionResult ProductRiskList()
        {
            return View();
        }
        public IActionResult ProductRiskDetail()
        {
            return View();
        }

        [Route("ProductRisk/Create")]
        public IActionResult ProductRiskCreate()
        {
            return View("ProductRiskEdit");
        }
        [Route("ProductRisk/Edit/{id?}")]
        public IActionResult ProductRiskEdit()
        {
            return View();
        }
    }
}
