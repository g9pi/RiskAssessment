using System;

namespace RiskAssessment.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public ErrorViewModel(int code, string message, bool isPartial = false)
        {
            Code = code;
            Message = message;
            IsPartial = isPartial;
        }

        public int? Code { get; internal set; }
        public string Message { get; internal set; }
        public bool IsPartial { get; internal set; }
    }
}
