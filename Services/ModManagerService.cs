using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            InstallModsAsync(mods, targetPath, null).Wait();
        }

        public async Task InstallModsAsync(List<ModInfo> mods, string targetPath, IProgress<InstallProgress> progress)
        {
            await Task.Run(async () =>
            {
                // Создаем целевую папку если её нет
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                // Удаляем ВСЁ из целевой папки перед установкой
                progress?.Report(new InstallProgress
                {
                    CurrentMod = "Cleaning target folder...",
                    Installed = 0,
                    Total = 1,
                    Percentage = 0
                });

                // Список служебных файлов Vortex, которые нужно сохранить
                var vortexFilesToKeep = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "snapshot.json",
                    "rename_folders.py",
                    "update_snapshot.py",
                    "update_deployment.py"
                };

                // Удаляем ВСЕ папки из целевой папки
                var existingDirs = Directory.GetDirectories(targetPath);
                foreach (var dir in existingDirs)
                {
                    var dirName = Path.GetFileName(dir);
                    
                    try
                    {
                        await Task.Run(() =>
                        {
                            // Сначала снимаем атрибуты только для чтения, если они есть
                            var dirInfo = new DirectoryInfo(dir);
                            RemoveReadOnlyAttributes(dirInfo);
                            Directory.Delete(dir, true);
                        });
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to delete directory {dir}: {ex.Message}");
                        throw new Exception($"Failed to delete directory: {dirName}. Error: {ex.Message}. Make sure the folder is not open in another program.", ex);
                    }
                }

                // Удаляем ВСЕ файлы, кроме служебных Vortex
                var existingFiles = Directory.GetFiles(targetPath);
                foreach (var file in existingFiles)
                {
                    var fileName = Path.GetFileName(file);
                    
                    // Пропускаем только служебные файлы Vortex
                    bool isVortexFile = fileName.StartsWith("vortex.", StringComparison.OrdinalIgnoreCase) ||
                                       fileName.StartsWith("snapshot_", StringComparison.OrdinalIgnoreCase) ||
                                       vortexFilesToKeep.Contains(fileName);
                    
                    if (isVortexFile)
                    {
                        continue;
                    }

                    try
                    {
                        await Task.Run(() =>
                        {
                            var fileInfo = new FileInfo(file);
                            if (fileInfo.Exists)
                            {
                                fileInfo.Attributes = FileAttributes.Normal;
                                File.Delete(file);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to delete file {file}: {ex.Message}");
                        throw new Exception($"Failed to delete file: {fileName}. Error: {ex.Message}. Make sure the file is not open in another program.", ex);
                    }
                }

                // Копируем только включенные моды в правильном порядке
                var enabledMods = mods.Where(m => m.IsEnabled).OrderBy(m => m.Order).ToList();
                int total = enabledMods.Count;
                int installed = 0;

                foreach (var mod in enabledMods)
                {
                    var targetFolderName = mod.GetTargetFolderName();
                    var targetModPath = Path.Combine(targetPath, targetFolderName);

                    if (!Directory.Exists(targetModPath))
                    {
                        Directory.CreateDirectory(targetModPath);
                    }

                    // Обновляем прогресс
                    installed++;
                    var percentage = total > 0 ? (int)((double)installed / total * 100) : 0;
                    progress?.Report(new InstallProgress
                    {
                        CurrentMod = mod.Name,
                        Installed = installed,
                        Total = total,
                        Percentage = percentage
                    });

                    // Копируем все файлы из исходной папки мода
                    await CopyDirectoryAsync(mod.SourcePath, targetModPath);
                }
            });
        }

        private void CopyDirectory(string sourceDir, string targetDir)
        {
            CopyDirectoryAsync(sourceDir, targetDir).Wait();
        }

        private async Task CopyDirectoryAsync(string sourceDir, string targetDir)
        {
            await Task.Run(async () =>
            {
                Directory.CreateDirectory(targetDir);

                // Копируем файлы
                var files = Directory.GetFiles(sourceDir);
                foreach (var file in files)
                {
                    await Task.Run(() =>
                    {
                        var fileName = Path.GetFileName(file);
                        var targetFilePath = Path.Combine(targetDir, fileName);
                        File.Copy(file, targetFilePath, true);
                    });
                }

                // Рекурсивно копируем подпапки
                var dirs = Directory.GetDirectories(sourceDir);
                foreach (var dir in dirs)
                {
                    var dirName = Path.GetFileName(dir);
                    var targetSubDir = Path.Combine(targetDir, dirName);
                    await CopyDirectoryAsync(dir, targetSubDir);
                }
            });
        }

        private void RemoveReadOnlyAttributes(DirectoryInfo directoryInfo)
        {
            try
            {
                if (!directoryInfo.Exists) return;

                directoryInfo.Attributes &= ~FileAttributes.ReadOnly;

                // Убираем атрибуты только для чтения у всех файлов в папке
                foreach (var file in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
                {
                    file.Attributes &= ~FileAttributes.ReadOnly;
                }

                // Рекурсивно для всех подпапок
                foreach (var subDir in directoryInfo.GetDirectories("*", SearchOption.AllDirectories))
                {
                    subDir.Attributes &= ~FileAttributes.ReadOnly;
                }
            }
            catch
            {
                // Игнорируем ошибки при изменении атрибутов
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

