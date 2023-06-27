using RiskAssessment.Models.Shared;
using System.Collections.Generic;

namespace RiskAssessment.Models.Filters
{
    public class UserFilterModel : ListModel<UserModel>
    {
        public string Keyword { get; set; }      
        public string Group { get; set; }
        public string Section { get; set; }
        public string Division { get; set; }
        public bool? Isactive { get; set; }

    }
}
