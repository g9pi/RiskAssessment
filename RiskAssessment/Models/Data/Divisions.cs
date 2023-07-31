using System;
using System.Collections.Generic;

namespace RiskAssessment.Models.Data
{
    public class Divisions
    {
        public int? DivisionId { get; set; }
        public string DivisionName { get; set; }
        public List<SectionsModel> Sections { get; set; }
        public bool IsActive { get; set; }
        public string CreateUser { get; set; }
        public string CreateIp { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyUser { get; set; }
        public string ModifyIp { get; set; }
        public DateTime? ModifyDate { get; set; }
        public List<int?> ReviewerIBP_LV1 { get; set; }
        public List<int?> ReviewerIBP_LV2 { get; set; }
    }
}
