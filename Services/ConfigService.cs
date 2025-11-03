using System.IO;
using Newtonsoft.Json;
using Stalker2ModManager.Models;

namespace Stalker2ModManager.Services
{
    public class ConfigService
    {
        private readonly string _configPath = "mods_config.json";

        public ModConfig LoadConfig()
        {
            if (!File.Exists(_configPath))
            {
                return new ModConfig();
            }

            try
            {
                var json = File.ReadAllText(_configPath);
                return JsonConvert.DeserializeObject<ModConfig>(json) ?? new ModConfig();
            }
            catch
            {
                return new ModConfig();
            }
        }

        public void SaveConfig(ModConfig config)
        {
            try
            {
                var json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(_configPath, json);
            }
            catch
            {
                // Ошибка сохранения
            }
        }
    }
}

