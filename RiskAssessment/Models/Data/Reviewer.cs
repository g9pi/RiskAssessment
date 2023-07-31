using System;
using System.Collections.Generic;

namespace RiskAssessment.Models.Data
{
    public class Reviewer
    {
        public int? RowId { get; set; }
        public int? SectionId { get; set; }
        public short? LV { get; set; }
        public int? UserId { get; set; }
        public string ReviewType { get; set; }
        public string CreateUser { get; set; }
        public string CreateIp { get; set; }
        public DateTime? CreateDate { get; set; }

    }
    public class ReviewerInput
    {
        public int? SectionId { get; set; }
        public short? LV { get; set; }
        public List<int?> UserIds { get; set; }
        public string ReviewType { get; set; }
        public string CreateUser { get; set; }
        public string CreateIp { get; set; }
    }
}
