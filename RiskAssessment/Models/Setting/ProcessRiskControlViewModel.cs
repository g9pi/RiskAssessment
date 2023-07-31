using Microsoft.AspNetCore.Mvc.Rendering;
using RiskAssessment.Models.Data;
using System.Collections.Generic;

namespace RiskAssessment.Models.Setting
{
    public class ProcessRiskControlViewModel
    {
        public Divisions Division { get; set; }
        public List<Divisions> Divisions { get; set; }
        public List<SelectListItem> Division_ddl { get; set; } = new List<SelectListItem> {
                new SelectListItem {Text = "AAP",Value = "AAP" },
                new SelectListItem {Text = "CB",Value = "CB" },
                new SelectListItem {Text = "CE",Value = "CE" },
                new SelectListItem {Text = "General Office",Value = "General Office" },
                new SelectListItem {Text = "General Production",Value = "General Production" },
                new SelectListItem {Text = "GL",Value = "GL" },
                new SelectListItem {Text = "HA",Value = "HA" },
                new SelectListItem {Text = "MS",Value = "MS" },
                new SelectListItem {Text = "MSB",Value = "MSB" },
                new SelectListItem {Text = "PVD",Value = "PVD" },
                new SelectListItem {Text = "SS",Value = "SS" },
         };
        public List<SelectListItem> ToDDL(List<Divisions> Divisions)
        {
            List<SelectListItem> Division_ddl = new List<SelectListItem>();
            foreach(var item in Divisions)
            {
                Division_ddl.Add(new SelectListItem
                {
                    Text = item.DivisionName,
                    Value = item.DivisionName
                });
            }
            return Division_ddl; 
        }
    }
}
