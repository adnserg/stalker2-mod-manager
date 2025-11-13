namespace Stalker2ModManager.Models
{
    public class ModConfig
    {
        public string VortexPath { get; set; } = string.Empty;
        public string TargetPath { get; set; } = string.Empty;
        public double WindowWidth { get; set; } = 800;
        public double WindowHeight { get; set; } = 600;
        public string Language { get; set; } = "en";
        public string CustomLocalizationPath { get; set; } = string.Empty;
        public string LastImportOrderPath { get; set; } = string.Empty;
        public string LastExportOrderPath { get; set; } = string.Empty;
        public bool ConsiderModVersion { get; set; } = true;
        public bool ValidateGamePathEnabled { get; set; } = true;
    }
}

