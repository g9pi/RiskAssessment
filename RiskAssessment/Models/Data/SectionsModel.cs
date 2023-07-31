using System;
using System.Collections.Generic;

namespace RiskAssessment.Models.Data
{
    public class SectionsModel
    {
        public int? SectionId { get; set; }
        public string Sectionname { get; set; }
        public string Division { get; set; }
        public bool Selected { get; set; }
        public bool IsActive { get; set; }
        public List<Reviewer> ReviewerLV1 { get; set; }
        public List<int?> LV1Value_4DDL { get; set; }
        public List<Reviewer> ReviewerLV2 { get; set; }
        public List<int?> LV2Value_4DDL { get; set; }
        public string CreateUser { get; set; }
        public string CreateIp { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyUser { get; set; }
        public string ModifyIp { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
    
    
}
