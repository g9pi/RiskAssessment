using System;

namespace RiskAssessment.Models.Data
{
    public class EmployeeModel
    {
        //public EmployeeModel()
        //{
        //    if(!string.IsNullOrEmpty(SectionNo))
        //        setDivision();
        //}
        public int? EmployeeId { get; set; }
        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNameTH { get; set; }
        public string CitizenNo { get; set; }
        public string Gender { get; set; }
        public string DivisionNo { get; set; }
        public string Division { get; set; }
        public string DepartmentNo { get; set; }
        public string SectionNo { get; set; }
        public string Level { get; set; }
        public string Position { get; set; }
        public DateTime? JoinedDate { get; set; }
        public string EmployeeStatus { get; set; }
        public void setDivision()
        {
            string text = null;
            if (SectionNo.Substring(0, 2).Equals("13"))
            {
                text = SectionNo.Substring(0, 2);
            }
            else if (SectionNo.Substring(0, 2).Equals("P9"))
            {
                text = SectionNo.Substring(0, 2);
            }                      
            else
            {
                text = SectionNo.Substring(0, 1);
            }

            switch (text)
            {
                case "1": this.Division = "CE"; break;
                case "13": this.Division = "AAP"; break;
                case "2": this.Division = "AAP"; break;
                case "7": this.Division = "SS"; break;
                case "8": this.Division = "HA"; break;
                case "9": this.Division = "CB"; break;
                case "P9": this.Division = "MSB"; break;
                case "Z": this.Division = "MS"; break;
                case "Y": this.Division = "GL"; break;               
                case "6": this.Division = "PVD"; break;
                case "G": this.Division = "General Office"; break;
                case "P": this.Division = "General Production"; break;
                default: this.Division = "Unknown"; break;
            }
        }
    }
    
}
