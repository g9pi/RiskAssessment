namespace RiskAssessment.Models.Data
{
    public struct AccountClaim
    {
        public const string UserId = "UserId";
        public const string Username = "Username";
        public const string Email = "Email";
        public const string Name = "Name";
        public const string UserGroup = "Group";
        public const string Division = "Division";
        public const string Section = "Section";
    }

    public struct MaxSize
    {
        public const int Image = 5242880; // Bytes ~ 5MB
    }
    public struct RelativePath
    {
        public const string Commons = "wwwroot\\assets\\commons\\";
        public const string File = "wwwroot\\assets\\files\\";
    }

}
