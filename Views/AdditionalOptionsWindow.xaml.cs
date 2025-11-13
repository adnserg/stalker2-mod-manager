using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Stalker2ModManager.Services;

namespace Stalker2ModManager.Views
{
    public partial class AdditionalOptionsWindow : Window
    {
        public bool SortBySnapshot { get; private set; }
        public string JsonFilePath { get; private set; } = string.Empty;
        public bool ConsiderModVersion { get; private set; }
        public bool ValidateGamePathEnabled { get; private set; }
        public bool LocalizationChanged { get; private set; }

        public AdditionalOptionsWindow()
        {
            InitializeComponent();
            // Initialize settings from config
            var configService = new ConfigService();
            var cfg = configService.LoadPathsConfig();
            ConsiderModVersion = cfg.ConsiderModVersion;
            ValidateGamePathEnabled = cfg.ValidateGamePathEnabled;
            
            if (ConsiderModVersionCheckBox != null)
            {
                ConsiderModVersionCheckBox.IsChecked = ConsiderModVersion;
                ConsiderModVersionCheckBox.Content = LocalizationService.Instance.GetString("ConsiderModVersion");
            }

            if (ValidateGamePathCheckBox != null)
            {
                ValidateGamePathCheckBox.IsChecked = ValidateGamePathEnabled;
            }

            // Localize buttons
            if (OkButton != null) OkButton.Content = LocalizationService.Instance.GetString("OK");
            if (CancelButton != null) CancelButton.Content = LocalizationService.Instance.GetString("Cancel");
            
            // Localize static texts
            SortBySnapshotCheckBox.Content = LocalizationService.Instance.GetString("SortByFile");
            
            if (LoadCustomLocalizationButton != null)
                LoadCustomLocalizationButton.Content = LocalizationService.Instance.GetString("LoadCustomLocalization");
            if (ResetLocalizationButton != null)
                ResetLocalizationButton.Content = LocalizationService.Instance.GetString("ResetLocalization");
        }

        private void SortBySnapshotCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            JsonFilePathTextBox.IsEnabled = true;
            BrowseJsonButton.IsEnabled = true;
            ApplyButton.IsEnabled = true;
        }

        private void SortBySnapshotCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            JsonFilePathTextBox.IsEnabled = false;
            BrowseJsonButton.IsEnabled = false;
            ApplyButton.IsEnabled = false;
            JsonFilePathTextBox.Text = string.Empty;
        }

        private void ConsiderModVersionCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ConsiderModVersion = true; // Сохраняем только по OK
        }

        private void ConsiderModVersionCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ConsiderModVersion = false; // Сохраняем только по OK
        }

        private void ValidateGamePathCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ValidateGamePathEnabled = true;
        }

        private void ValidateGamePathCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ValidateGamePathEnabled = false;
        }

        private void LoadCustomLocalization_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var dialog = new OpenFileDialog();
                dialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                dialog.Title = LocalizationService.Instance.GetString("SelectJsonFile");

                // Устанавливаем начальную директорию - последний использованный путь, Localization папка или рабочая директория
                var configService = new ConfigService();
                var pathsConfig = configService.LoadPathsConfig();
                string? initialDir = null;
                
                if (!string.IsNullOrWhiteSpace(pathsConfig.CustomLocalizationPath) && File.Exists(pathsConfig.CustomLocalizationPath))
                {
                    initialDir = Path.GetDirectoryName(pathsConfig.CustomLocalizationPath);
                    dialog.FileName = Path.GetFileName(pathsConfig.CustomLocalizationPath);
                }
                else
                {
                    var currentDir = Environment.CurrentDirectory;
                    var localizationDir = Path.Combine(currentDir, "Localization");
                    if (Directory.Exists(localizationDir))
                    {
                        initialDir = localizationDir;
                    }
                    else if (Directory.Exists(currentDir))
                    {
                        initialDir = currentDir;
                    }
                }
                
                if (!string.IsNullOrEmpty(initialDir))
                {
                    dialog.InitialDirectory = initialDir;
                }

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    LocalizationService.Instance.LoadFromExternalFile(dialog.FileName);
                    LocalizationChanged = true;
                    var localization = LocalizationService.Instance;
                    WarningWindow.Show(
                        localization.GetString("ConfigSavedSuccess"),
                        localization.GetString("Success"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                var localization = LocalizationService.Instance;
                WarningWindow.Show(
                    $"Error loading localization: {ex.Message}",
                    localization.GetString("Error"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ResetLocalization_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LocalizationService.Instance.ResetToEmbedded();
                LocalizationChanged = true;
                var localization = LocalizationService.Instance;
                WarningWindow.Show(
                    localization.GetString("ConfigSavedSuccess"),
                    localization.GetString("Success"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                var localization = LocalizationService.Instance;
                WarningWindow.Show(
                    $"Error resetting localization: {ex.Message}",
                    localization.GetString("Error"),
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            // Применяем и сохраняем настройки
            var configService = new ConfigService();
            var cfg = configService.LoadPathsConfig();
            cfg.ConsiderModVersion = ConsiderModVersionCheckBox.IsChecked ?? cfg.ConsiderModVersion;
            cfg.ValidateGamePathEnabled = ValidateGamePathCheckBox.IsChecked ?? cfg.ValidateGamePathEnabled;
            configService.SavePathsConfig(cfg);

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BrowseJsonFile_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "All supported files (*.json;*.txt)|*.json;*.txt|JSON files (*.json)|*.json|Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Select JSON or TXT file with mod order"
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                JsonFilePathTextBox.Text = dialog.FileName;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            SortBySnapshot = SortBySnapshotCheckBox.IsChecked ?? false;
            JsonFilePath = JsonFilePathTextBox.Text;

            if (SortBySnapshot && string.IsNullOrWhiteSpace(JsonFilePath))
            {
                var localization = LocalizationService.Instance;
                WarningWindow.Show(localization.GetString("SelectFile"), localization.GetString("Error"), MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void TitleBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}

