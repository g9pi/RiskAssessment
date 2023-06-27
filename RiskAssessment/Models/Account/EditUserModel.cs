using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace RiskAssessment.Models.Account
{
    public class EditUserModel
    {
        
        public UserModel User { get; set; }
        public List<SelectListItem> Section_ddl { get; set; }
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
        public EditUserModel()
        {
            
        }
        //public EditUserModel(bool isCreate)
        //{
        //    this.IsCreate = isCreate;
        //}
    }
}
