using RiskAssessment.Models.Data;
using RiskAssessment.Service.IService;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace RiskAssessment.Service
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(EmailModel email)
        {
            try
            {
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(email.SenderUser, email.SenderName);
                email.Receivers.ForEach((r) => { mailMessage.To.Add(new MailAddress(r)); });
                email.CCReceivers?.ForEach((r) => { mailMessage.CC.Add(new MailAddress(r)); });
                email.BCCReceivers?.ForEach((r) => { mailMessage.Bcc.Add(new MailAddress(r)); });

                mailMessage.Subject = email.Subject;
                mailMessage.IsBodyHtml = email.IsBodyHtml;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mailMessage.Body = email.Message;



                var credential = new NetworkCredential(email.SenderUser, email.SenderPassword);
                var smtpClient = new SmtpClient();
                smtpClient.Host = email.Host;
                smtpClient.Port = email.Port;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = credential;
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(mailMessage);

                smtpClient.Dispose();
                mailMessage.Dispose();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
