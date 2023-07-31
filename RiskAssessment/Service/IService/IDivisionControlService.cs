using RiskAssessment.Models.Data;
using System.Collections.Generic;

namespace RiskAssessment.Service.IService
{
    public interface IDivisionControlService
    {
        public List<Divisions> getDivisions();
        public void saveDivision(Divisions input);
        public SectionsModel getSectionsbyId(int sectionId);
        public Divisions getDivisionWithSec(string divisionName);

        public void saveSection(SectionsModel input);
        public void updateReviewer(ReviewerInput input);

    }
}
