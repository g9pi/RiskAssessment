using Microsoft.AspNetCore.Mvc.Rendering;
using RiskAssessment.Models.Data;
using System.Collections.Generic;

namespace RiskAssessment.Models.Setting
{
    public class DivAndResponseSettingViewModel
    {
        public Divisions Division { get; set; }
        public List<Divisions> Divisions { get; set; }
        public List<SelectListItem> Users_ddl { get; set; }
    }
}
