using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RiskAssessment.Models.ProcessRisk;
using RiskAssessment.Models.Setting;
using RiskAssessment.Service.IService;

namespace RiskAssessment.Controllers
{
    public class ProcessRiskController : BaseController
    {
        public const string Name = "ProcessRisk";
        public const string ActionProcessRisk = nameof(ProcessRisks);
        public const string ActionCreateProcess = nameof(CreateProcess);
        public const string ActionEditProcess = nameof(EditProcess);
        public const string ActionProcessRiskDetail = nameof(ProcessRiskDetail);

        public const string ActionCreateRisk = nameof(CreateRisk);


        private readonly IUserAccountService _userAccountService;
        private readonly IDivisionControlService _divisionControlService;
        public ProcessRiskController(IUserAccountService userAccountService, IConfiguration configuration, IDivisionControlService divisionControlService) : base(configuration)
        {
            _userAccountService = userAccountService;
            _divisionControlService = divisionControlService;

        }


        [Route("ProcessRisks")]
        public IActionResult ProcessRisks(string DivisionName, string SectionName)
        {
            ProcessRiskViewModel model = new ProcessRiskViewModel();
            model.Divisions = _divisionControlService.getDivisions();
            var profile = _userAccountService.GetUserByUsername(_accountUsername);
            var Userdivision = profile.Division;
            var UserSection = profile.Section;
            Userdivision = Userdivision.Contains("General") ? "General" : Userdivision;
            if (DivisionName == null)
            {
                model.Division = _divisionControlService.getDivisionWithSec(Userdivision);
                if (model.Division != null)
                {
                    var Section_index = model.Division.Sections.FindIndex(s => s.Sectionname == UserSection);
                    if (Section_index > -1 && Section_index < model.Division.Sections.Count)
                    {
                        model.Division.Sections[Section_index].Selected = true;
                    }
                    else
                    {
                        if (model.Division.Sections != null && model.Division.Sections.Count > 0)
                        {
                            model.Division.Sections[0].Selected = true;
                        }
                    }
                }
                else
                {
                    model.Division = _divisionControlService.getDivisionWithSec(model.Divisions[0].DivisionName);
                    if (model.Division.Sections != null && model.Division.Sections.Count > 0)
                    {

                        model.Division.Sections[0].Selected = true;
                    }
                }


            }
            else
            {

                model.Division = _divisionControlService.getDivisionWithSec(DivisionName);
                var Section_index = model.Division.Sections.FindIndex(s => s.Sectionname == SectionName);
                if (Section_index > -1 && Section_index < model.Division.Sections.Count)
                {
                    model.Division.Sections[Section_index].Selected = true;
                }
                else
                {
                    if (model.Division.Sections != null && model.Division.Sections.Count > 0)
                    {

                        model.Division.Sections[0].Selected = true;
                    }
                }
            }
            model.Division_ddl = model.ToDDL(model.Divisions);
            int div_index = -1;
            div_index = DivisionName == null ? div_index = model.Division_ddl.FindIndex(d => d.Value == Userdivision) : div_index = model.Division_ddl.FindIndex(d => d.Value == DivisionName);

            if (div_index > -1 && div_index < model.Division_ddl.Count)
            {
                model.Division_ddl[div_index].Selected = true;
            }
            else
            {
                model.Division_ddl[0].Selected = true;
            }
            return View(model);
        }
        [Route("ProcessRisks/EditProcess/{id?}")]
        public IActionResult EditProcess(int Id)
        {
            return View();
        }
        [Route("ProcessRisks/Detail/{id?}")]
        public IActionResult ProcessRiskDetail(int Id)
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
