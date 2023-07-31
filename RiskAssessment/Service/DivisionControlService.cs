using RiskAssessment.Models.Filters;
using RiskAssessment.Models;
using RiskAssessment.Repositorys.IRepositorys;
using RiskAssessment.Service.IService;
using System.Collections.Generic;
using RiskAssessment.Models.Data;
using System.Linq;
using static System.Collections.Specialized.BitVector32;

namespace RiskAssessment.Service
{
    public class DivisionControlService : IDivisionControlService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IDivisionControlRepository _divisionControlRepository;
        public DivisionControlService(IUserAccountRepository userAccountRepository, IDivisionControlRepository divisionControlRepository)
        {
            _userAccountRepository = userAccountRepository;
            _divisionControlRepository = divisionControlRepository;
        }
        public List<Divisions> getDivisions()
        {
            List<Divisions> divisions = _divisionControlRepository.getDivision();      
            return divisions;
        }
        public Divisions getDivisionWithSec(string divisionName)
        {
            Divisions Division = _divisionControlRepository.getDivision().Where(div => div.DivisionName == divisionName).FirstOrDefault();
            Division.Sections = GetSectionsByDivision(Division.DivisionName);
            List<Reviewer> reviewers = _divisionControlRepository.getReviewer();
            Division.ReviewerIBP_LV1 = reviewers.Where(D => D.SectionId == Division.DivisionId && D.LV == 1 && D.ReviewType == "DIV").Select(U => U.UserId).ToList();
            Division.ReviewerIBP_LV2 = reviewers.Where(D => D.SectionId == Division.DivisionId && D.LV == 2 && D.ReviewType == "DIV").Select(U => U.UserId).ToList();
            foreach (var section in Division.Sections)
            {
                //section.ReviewerLV1 = .ToList();
                section.LV1Value_4DDL = reviewers.Where(D => D.SectionId == section.SectionId && D.LV == 1 && D.ReviewType == "SEC").Select(U => U.UserId).ToList();
                //section.ReviewerLV2 =.ToList();
                section.LV2Value_4DDL = reviewers.Where(D => D.SectionId == section.SectionId && D.LV == 2 && D.ReviewType == "SEC").Select(U => U.UserId).ToList();
            }
            return Division;
        }
        public SectionsModel getSectionsbyId(int sectionId)
        {
            SectionsModel sections = _divisionControlRepository.getSection().Where(i =>i.SectionId == sectionId).FirstOrDefault();
            return sections;
        }
        public List<SectionsModel> GetSectionsByDivision(string  divisionName)
        {
            List<SectionsModel> sections = _divisionControlRepository.getSection().Where(div => div.Division == divisionName).ToList(); 
            
            return sections;
        }
        public void saveDivision(Divisions input)
        {
            if(input.DivisionId == null)
            {
                input.IsActive = true;
                _divisionControlRepository.insertDivision(input);
            }
            else
            {
                _divisionControlRepository.updateDivision(input);
            }
        }
        public void saveSection(SectionsModel input)
        {
            if(input.SectionId == null)
            {
                input.IsActive = true;
                _divisionControlRepository.insertSection(input);
            }
            else
            {
                //update
                
                _divisionControlRepository.updateSection(input);
            }
            
        }

        public void updateReviewer(ReviewerInput input)
        {
            _divisionControlRepository.DeleteReviewerBySectionId(input.SectionId,input.LV,input.ReviewType);
            if(input.UserIds != null &&  input.UserIds.Count > 0)
            {
                _divisionControlRepository.UpdateReviewer(input);
            }
            
        }
    }
}
