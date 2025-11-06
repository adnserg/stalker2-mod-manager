using System;
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

        public AdditionalOptionsWindow()
        {
            InitializeComponent();
            // Initialize ConsiderModVersion from config
            var configService = new ConfigService();
            var cfg = configService.LoadPathsConfig();
            ConsiderModVersion = cfg.ConsiderModVersion;
            if (ConsiderModVersionCheckBox != null)
            {
                ConsiderModVersionCheckBox.IsChecked = ConsiderModVersion;
                ConsiderModVersionCheckBox.Content = LocalizationService.Instance.GetString("ConsiderModVersion");
            }

            // Localize buttons
            if (OkButton != null) OkButton.Content = LocalizationService.Instance.GetString("OK");
            if (CancelButton != null) CancelButton.Content = LocalizationService.Instance.GetString("Cancel");
            
            // Localize static texts
            SortBySnapshotCheckBox.Content = LocalizationService.Instance.GetString("SortByFile");
            // The label and buttons for file path row
            // We won't rename label here, but in main window localization keys exist
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

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            // Применяем и сохраняем настройки
            var configService = new ConfigService();
            var cfg = configService.LoadPathsConfig();
            cfg.ConsiderModVersion = ConsiderModVersionCheckBox.IsChecked ?? cfg.ConsiderModVersion;
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

