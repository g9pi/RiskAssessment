using RiskAssessment.Models.Data;
using System.Threading.Tasks;

namespace RiskAssessment.Service.IService
{
    public interface IEmailService
    {
        public Task SendEmail(EmailModel email);
    }
}
