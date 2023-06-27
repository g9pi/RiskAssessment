using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml.Drawing.Chart;
using RiskAssessment.Models.Filters;
using RiskAssessment.Models.Shared;
using System.Collections.Generic;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace RiskAssessment.Models.Account
{
    public class AccountListModel : ListModel<UserModel>
    {
        public List<UserModel> Users { get; set; }
        public List<SelectListItem> Section_ddl { get; set; }
        public List<SelectListItem> Division_ddl { get; set; } =  new List<SelectListItem> {
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
        public AccountListModel()
        {
            
        }
        public string Keyword { get; set; }
        public string Group { get; set; }
        public string Section { get; set; }
        public string Division { get; set; }
        public bool? Isactive { get; set; }

        public AccountListModel ToModel(UserFilterModel filter)
        {
            return new AccountListModel
            {
                //Users = Users,
                Page = filter.Page,
                PageSize = filter.PageSize,
                Keyword = filter.Keyword,
                Group = filter.Group,
                Division = filter.Division,
                Section = filter.Section,
                Isactive = filter.Isactive,
            };
        }
        public AccountListModel ToModel()
        {
            return new AccountListModel
            {
                Page = Page,
                PageSize = PageSize,
                Keyword = Keyword,
                Group = Group,
                Division = Division,
                Section = Section,
                Isactive = Isactive,               
            };
        }
    }
}
