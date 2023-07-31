namespace RiskAssessment.Models.Admin
{
    public class BackupModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }

        public string SizeString()
        {
            string[] unit = new[] { "B", "KB", "MB", "GB" };
            int unitPointer = 0;
            double totalSize = Size;
            while (totalSize >= 1024)
            {
                totalSize = totalSize / 1024;
                unitPointer += 1;
            }
            return totalSize.ToString("N") + $" {unit[unitPointer]}";
        }
    }
}
