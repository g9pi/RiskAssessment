using System;

namespace RiskAssessment.Models.Shared
{
    public static class BaseModel
    {
        public static string ToAutoString(this int value)
        {
            return value.ToString("N0");
        }

        public static string ToAutoString(this double value, int digit = 2)
        {
            if (Math.Abs(value) % 1 > 0)
            {
                return value.ToString("N" + digit);
            }
            return value.ToString("N");
        }
    }
}
