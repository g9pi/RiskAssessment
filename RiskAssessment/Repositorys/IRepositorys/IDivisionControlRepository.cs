using RiskAssessment.Models.Data;
using System.Collections.Generic;

namespace RiskAssessment.Repositorys.IRepositorys
{
    public interface IDivisionControlRepository
    {
        //DIVISION
        public List<Divisions> getDivision();
        public void insertDivision(Divisions input);
        public void updateDivision(Divisions input);
        //SECTION
        public List<SectionsModel> getSection();
        public void insertSection(SectionsModel input);
        public void updateSection(SectionsModel input);

        //REVIEWER
        public List<Reviewer> getReviewer();
        public void UpdateReviewer(ReviewerInput input);
        public void DeleteReviewerBySectionId(int? SectionId, short? level, string ReviewType);
    }
}
