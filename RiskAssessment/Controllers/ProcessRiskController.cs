using Microsoft.AspNetCore.Mvc;

namespace RiskAssessment.Controllers
{
    public class ProcessRiskController : Controller
    {
        public const string Name = "ProcessRisk";
        public const string ActionProcessRisk = nameof(ProcessRisks);
        public const string ActionCreateProcess = nameof(CreateProcess);
        public const string ActionEditProcess = nameof(EditProcess);

        public const string ActionCreateRisk = nameof(CreateRisk);

        [Route("ProcessRisks")]
        public IActionResult ProcessRisks()
        {
            return View();
        }
        [Route("ProcessRisks/EditProcess/{id?}")]
        public IActionResult EditProcess(int Id)
        {
            return View();
        }
        [Route("ProcessRisks/CreateProcess")]
        public IActionResult CreateProcess(int Id)
        {
            return View("EditProcess");
        }

        [Route("ProcessRisks/CreateRisk/{id?}")]
        public IActionResult CreateRisk(int ProcessId)
        {
            return View("EditRisk");
        }
        public IActionResult EditRisk(int RiskId)
        {
            return View();
        }
    }
}
