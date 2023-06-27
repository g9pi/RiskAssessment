using Microsoft.AspNetCore.Mvc.Rendering;
using RiskAssessment.Models.Data;
using System.Collections.Generic;

namespace RiskAssessment.Service.IService
{
    public interface IExternalService
    {
        public void UpdateUserStatusFromEmployeeNo();
        public List<EmployeeModel> getEmployee();
        public List<SelectListItem> getSections();
    }
}
