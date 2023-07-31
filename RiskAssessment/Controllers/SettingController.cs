using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using RiskAssessment.Models;
using RiskAssessment.Models.Data;
using RiskAssessment.Models.Setting;
using RiskAssessment.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace RiskAssessment.Controllers
{
    [Authorize]
    public class SettingController : BaseController
    {
        public const string Name = "Setting";
        //View
        public const string ActionDivisionsAndResponse = nameof(DivisionsAndResponse);
        public const string ActionProcessRiskControl = nameof(ProcessRiskControl);
        public const string ActionProcessRiskCreate = nameof(ProcessRiskCreate);
        public const string ActionProcessRiskEdit = nameof(ProcessRiskEdit);
        public const string ActionProcessRiskFlow = nameof(ProcessRiskFlow);
        public const string ActionFlowCreate = nameof(FlowCreate);
        public const string ActionRisksControl = nameof(RisksControl);
        //API
        public const string ActionSaveSection = nameof(saveSection);
        public const string ActionSaveDivision = nameof(saveDivision);
        public const string ActionSaveSectionStatus = nameof(saveSectionStatus);
        public const string ActionUpdateReviewer = nameof(updateReviewer);

        //Service
        private readonly IUserAccountService _userAccountService;
        private readonly IDivisionControlService _divisionControlService;
        public SettingController(IUserAccountService userAccountService, IConfiguration configuration,IDivisionControlService divisionControlService) : base(configuration)
        {
            _userAccountService = userAccountService;
            _divisionControlService = divisionControlService;

        }
       
        public IActionResult ProcessRiskControl(string DivisionName,string SectionName)
        {
            ProcessRiskControlViewModel model = new ProcessRiskControlViewModel();
            model.Divisions = _divisionControlService.getDivisions();
            var profile = _userAccountService.GetUserByUsername(_accountUsername);
            var Userdivision = profile.Division;
            var UserSection = profile.Section;
            Userdivision = Userdivision.Contains("General") ? "General" : Userdivision;           
            if (DivisionName == null)
            {                               
                model.Division = _divisionControlService.getDivisionWithSec(Userdivision);
                if(model.Division != null)
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
                    if(model.Division.Sections != null && model.Division.Sections.Count > 0)
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

        #region Flow
        public IActionResult ProcessRiskFlow(string id)
        {
            return View();
        }
        [Route("ProcessRiskControl/CreateFlow")]
        public IActionResult FlowCreate()
        {
            return View("FlowEdit");
        }
        public IActionResult FlowEdit()
        {
            return View();
        }
        #endregion

        #region Risks
        public IActionResult RisksControl()
        {
            return View();
        }
        [Route("ProcessRiskControl/Risks")]
        public IActionResult ProcessRiskCreate()
        {
            return RedirectToAction(ActionProcessRiskEdit);
        }
        public IActionResult ProcessRiskEdit()
        {
            return View();
        }
        #endregion






        public IActionResult DivisionsAndResponse(string DivisionName)
        {
            DivAndResponseSettingViewModel model = new DivAndResponseSettingViewModel();
           
            model.Divisions = _divisionControlService.getDivisions();
            
            if (DivisionName == null)
            {
                var Userdivision = _accountDivision;
                if (Userdivision.Contains("General"))
                {
                    Userdivision = "General";
                }
                model.Division = _divisionControlService.getDivisionWithSec(Userdivision);

            }
            else
            {
                model.Division = _divisionControlService.getDivisionWithSec(DivisionName);
            }

            var users = _userAccountService.getUser(null);
            model.Users_ddl = new List<SelectListItem>();
            foreach(var user in users)
            {
                SelectListItem item = new SelectListItem
                {
                    Text = user.Name+" ("+user.Section+")",
                    Value = user.UserId.ToString(),
                    Disabled = !user.Isactive
                };
                model.Users_ddl.Add(item);
            }
            return View(model);
        }



        #region API

        [HttpPost]
        public JsonResult saveSection(SectionsModel input)
        {
            try
            {
                input.CreateUser = _accountUsername;
                input.CreateIp = _accountIp;
                input.ModifyUser = _accountUsername;
                input.ModifyIp = _accountIp;
                _divisionControlService.saveSection(input);               
                return Json(new { code = 200 });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, message = ex.Message, detail = ex.ToString() });
            }
        }
        [HttpPost]
        public JsonResult saveDivision(Divisions input)
        {
            try
            {
                input.CreateUser = _accountUsername;
                input.CreateIp = _accountIp;
                input.ModifyUser = _accountUsername;
                input.ModifyIp = _accountIp;
                _divisionControlService.saveDivision(input);
                return Json(new { code = 200 });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, message = ex.Message, detail = ex.ToString() });
            }
        }


        [HttpPost]
        public JsonResult saveSectionStatus(int sectionId,bool isActive)
        {
            try
            {
                SectionsModel input = new SectionsModel();
                //input = _divisionControlService
                input = _divisionControlService.getSectionsbyId(sectionId);
                
                input.IsActive = isActive;
                input.CreateUser = _accountUsername;
                input.CreateIp = _accountIp;
                input.ModifyUser = _accountUsername;
                input.ModifyIp = _accountIp;
                
                _divisionControlService.saveSection(input);
                return Json(new { code = 200 });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, message = ex.Message, detail = ex.ToString() });
            }
        }

        #endregion

        [HttpPost]
        public JsonResult updateReviewer(ReviewerInput input)
        {
            try{
                input.CreateUser = _accountUsername;
                input.CreateIp = _accountIp;
                _divisionControlService.updateReviewer(input);
                return Json(new { code = 200 });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, message = ex.Message, detail = ex.ToString() });
            }
           
        }
    }
}
