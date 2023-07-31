namespace RiskAssessment.Models
{
    public class Common
    {

        public struct UserGroup
        {
            public const string Webmaster = "Webmaster";
            public const string Admin = "Administrator";
            public const string Maintain = "Maintain";            
            public const string Read = "Reader";
          
        }
        public struct AbsolutePath
        {
            public const string Flows = "wwwroot\\files\\flows\\";          
            public const string UserImage = "wwwroot\\files\\profile_imgs\\";
          
        }
        public struct MaxSize
        {
            public const int Image = 5242880; // Bytes ~ 5MB
        }
    }
}
