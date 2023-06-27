using System.Collections.Generic;

namespace RiskAssessment.Models.Data
{
    public class EmailModel
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string SenderUser { get; set; }
        public string SenderName { get; set; }
        public string SenderPassword { get; set; }
        public List<string> Receivers { get; set; } = new List<string>();
        public List<string> CCReceivers { get; set; } = new List<string>();
        public List<string> BCCReceivers { get; set; } = new List<string>();

        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}
