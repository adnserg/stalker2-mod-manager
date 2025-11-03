using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stalker2ModManager.Models;

namespace Stalker2ModManager.Services
{
    public class ModManagerService
    {
        public List<ModInfo> LoadModsFromVortexPath(string vortexPath)
        {
            var mods = new List<ModInfo>();

            if (!Directory.Exists(vortexPath))
            {
                return mods;
            }

            var directories = Directory.GetDirectories(vortexPath);
            int order = 0;

            foreach (var dir in directories)
            {
                var dirInfo = new DirectoryInfo(dir);
                
                // Пропускаем служебные папки
                if (dirInfo.Name.StartsWith("__") || dirInfo.Name == "Better Vaulting")
                {
                    continue;
                }

                var modInfo = new ModInfo
                {
                    SourcePath = dir,
                    Name = dirInfo.Name,
                    Order = order++,
                    IsEnabled = true
                };

                mods.Add(modInfo);
            }

            return mods.OrderBy(m => m.Name).ToList();
        }

        public void InstallMods(List<ModInfo> mods, string targetPath)
        {
            // Создаем целевую папку если её нет
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            // Удаляем всё из целевой папки
            var existingDirs = Directory.GetDirectories(targetPath);
            foreach (var dir in existingDirs)
            {
                Directory.Delete(dir, true);
            }

            var existingFiles = Directory.GetFiles(targetPath);
            foreach (var file in existingFiles)
            {
                // Пропускаем служебные файлы Vortex
                var fileName = Path.GetFileName(file);
                if (fileName.StartsWith("vortex.") || 
                    fileName == "snapshot.json" || 
                    fileName.StartsWith("snapshot_") ||
                    fileName == "rename_folders.py" ||
                    fileName == "update_snapshot.py" ||
                    fileName == "update_deployment.py")
                {
                    continue;
                }

                File.Delete(file);
            }

            // Копируем только включенные моды в правильном порядке
            var enabledMods = mods.Where(m => m.IsEnabled).OrderBy(m => m.Order).ToList();

            foreach (var mod in enabledMods)
            {
                var targetFolderName = mod.GetTargetFolderName();
                var targetModPath = Path.Combine(targetPath, targetFolderName);

                if (!Directory.Exists(targetModPath))
                {
                    Directory.CreateDirectory(targetModPath);
                }

                // Копируем все файлы из исходной папки мода
                CopyDirectory(mod.SourcePath, targetModPath);
            }
        }

        private void CopyDirectory(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);

            // Копируем файлы
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var fileName = Path.GetFileName(file);
                var targetFilePath = Path.Combine(targetDir, fileName);
                File.Copy(file, targetFilePath, true);
            }

            // Рекурсивно копируем подпапки
            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                var dirName = Path.GetFileName(dir);
                var targetSubDir = Path.Combine(targetDir, dirName);
                CopyDirectory(dir, targetSubDir);
            }
        }

        public string GetDefaultTargetPath()
        {
            // Ищем установку S.T.A.L.K.E.R. 2 в стандартных местах Steam
            var steamPaths = new[]
            {
                @"E:\SteamLibrary\steamapps\common\S.T.A.L.K.E.R. 2 Heart of Chornobyl\Stalker2\Content\Paks\~mods",
                @"C:\Program Files (x86)\Steam\steamapps\common\S.T.A.L.K.E.R. 2 Heart of Chornobyl\Stalker2\Content\Paks\~mods",
                @"D:\SteamLibrary\steamapps\common\S.T.A.L.K.E.R. 2 Heart of Chornobyl\Stalker2\Content\Paks\~mods"
            };

            foreach (var path in steamPaths)
            {
                var gameDir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(path))));
                if (Directory.Exists(gameDir))
                {
                    return path;
                }
            }

            return string.Empty;
        }
    }
}

