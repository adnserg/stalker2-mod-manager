using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Stalker2ModManager.Models;
using Stalker2ModManager.Services;

namespace Stalker2ModManager
{
    public partial class MainWindow : Window
    {
        private readonly ModManagerService _modManagerService;
        private readonly ConfigService _configService;
        private ObservableCollection<ModInfo> _mods;

        public MainWindow()
        {
            InitializeComponent();
            _modManagerService = new ModManagerService();
            _configService = new ConfigService();
            _mods = new ObservableCollection<ModInfo>();
            ModsListBox.ItemsSource = _mods;

            LoadConfig();
        }

        private void BrowseVortexPath_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = "Select Vortex mods folder";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                VortexPathTextBox.Text = dialog.SelectedPath;
            }
        }

        private void BrowseTargetPath_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = "Select ~mods folder";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TargetPathTextBox.Text = dialog.SelectedPath;
            }
        }

        private void LoadMods_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(VortexPathTextBox.Text))
            {
                System.Windows.MessageBox.Show("Please select Vortex path first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var mods = _modManagerService.LoadModsFromVortexPath(VortexPathTextBox.Text);
                _mods.Clear();

                int order = 0;
                foreach (var mod in mods)
                {
                    mod.Order = order++;
                    _mods.Add(mod);
                }

                UpdateStatus($"Loaded {mods.Count} mods");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error loading mods: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var config = new ModConfig
                {
                    VortexPath = VortexPathTextBox.Text,
                    TargetPath = TargetPathTextBox.Text,
                    Mods = _mods.ToList()
                };

                _configService.SaveConfig(config);
                UpdateStatus("Config saved");
                System.Windows.MessageBox.Show("Config saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error saving config: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var config = _configService.LoadConfig();
                VortexPathTextBox.Text = config.VortexPath;
                TargetPathTextBox.Text = config.TargetPath;

                _mods.Clear();
                int order = 0;
                foreach (var mod in config.Mods)
                {
                    mod.Order = order++;
                    _mods.Add(mod);
                }

                UpdateStatus($"Loaded config with {config.Mods.Count} mods");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error loading config: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InstallMods_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TargetPathTextBox.Text))
            {
                System.Windows.MessageBox.Show("Please select target path first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = System.Windows.MessageBox.Show(
                "This will DELETE all files in ~mods folder and install only enabled mods. Continue?",
                "Warning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                _modManagerService.InstallMods(_mods.ToList(), TargetPathTextBox.Text);
                UpdateStatus($"Installed {_mods.Count(m => m.IsEnabled)} mods");
                System.Windows.MessageBox.Show("Mods installed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error installing mods: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (ModsListBox.SelectedItem is ModInfo selectedMod)
            {
                var index = _mods.IndexOf(selectedMod);
                if (index > 0)
                {
                    _mods.Move(index, index - 1);
                    UpdateOrders();
                    ModsListBox.SelectedIndex = index - 1;
                }
            }
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            if (ModsListBox.SelectedItem is ModInfo selectedMod)
            {
                var index = _mods.IndexOf(selectedMod);
                if (index < _mods.Count - 1)
                {
                    _mods.Move(index, index + 1);
                    UpdateOrders();
                    ModsListBox.SelectedIndex = index + 1;
                }
            }
        }

        private void UpdateOrders()
        {
            for (int i = 0; i < _mods.Count; i++)
            {
                _mods[i].Order = i;
            }
        }

        private void LoadConfig()
        {
            var config = _configService.LoadConfig();
            VortexPathTextBox.Text = config.VortexPath;
            
            if (string.IsNullOrWhiteSpace(config.TargetPath))
            {
                TargetPathTextBox.Text = _modManagerService.GetDefaultTargetPath();
            }
            else
            {
                TargetPathTextBox.Text = config.TargetPath;
            }

            if (config.Mods.Any())
            {
                _mods.Clear();
                int order = 0;
                foreach (var mod in config.Mods)
                {
                    mod.Order = order++;
                    _mods.Add(mod);
                }
            }
        }

        private void UpdateStatus(string message)
        {
            StatusTextBlock.Text = message;
        }
    }
}

