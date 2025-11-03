namespace Stalker2ModManager.Models
{
    public class InstallProgress
    {
        public string CurrentMod { get; set; } = string.Empty;
        public int Installed { get; set; }
        public int Total { get; set; }
        public int Percentage { get; set; }
    }
}

