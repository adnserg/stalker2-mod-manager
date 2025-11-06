using System;
using System.Windows;
using System.Windows.Input;
using Stalker2ModManager.Services;

namespace Stalker2ModManager.Views
{
    public partial class WarningWindow : Window
    {
        public MessageBoxResult Result { get; private set; } = MessageBoxResult.Cancel;

        public WarningWindow(string message, string title = "Warning", MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Warning, string? details = null)
        {
            InitializeComponent();
            
            var localization = LocalizationService.Instance;

            //Настраиваем иконку и заголовок в зависимости от типа
            if (icon == MessageBoxImage.Error)
            {
                txtIcon.Text = "❌";
                //TitleTextBlock.Text = localization.GetString("Error");
            }
            else if (icon == MessageBoxImage.Information)
            {
                txtIcon.Text = "ℹ️";
                //TitleTextBlock.Text = localization.GetString("Success");
            }
            else
            {
                txtIcon.Text = "⚠️";
                //TitleTextBlock.Text = localization.GetString("Warning");
            }

            TitleTextBlock.Text = title;
            
            MessageTextBlock.Text = message;
            
            if (!string.IsNullOrEmpty(details))
            {
                DetailsTextBlock.Text = details;
                DetailsTextBlock.Visibility = Visibility.Visible;
            }
            
            // Настраиваем кнопки в зависимости от типа диалога
            if (buttons == MessageBoxButton.OK)
            {
                CancelButton.Visibility = Visibility.Collapsed;
                OkButton.Content = localization.GetString("OK");
                OkButton.Width = 120;
            }
            else if (buttons == MessageBoxButton.YesNo)
            {
                CancelButton.Content = localization.GetString("No");
                OkButton.Content = localization.GetString("Yes");
            }
            else if (buttons == MessageBoxButton.OKCancel)
            {
                CancelButton.Content = localization.GetString("Cancel");
                OkButton.Content = localization.GetString("OK");
            }
            else if (buttons == MessageBoxButton.YesNoCancel)
            {
                // Порядок: Yes (OkButton), No (CancelButton), Cancel (ExtraButton)
                OkButton.Content = localization.GetString("Yes");
                CancelButton.Content = localization.GetString("No");
                ExtraButton.Content = localization.GetString("Cancel");
                ExtraButton.Visibility = Visibility.Visible;
            }
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Может быть No в некоторых режимах
            var localization = LocalizationService.Instance;
            if (CancelButton.Content.ToString() == localization.GetString("No"))
            {
                Result = MessageBoxResult.No;
            }
            else
            {
                Result = MessageBoxResult.Cancel;
            }
            Close();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Определяем результат в зависимости от типа кнопки
            var localization = LocalizationService.Instance;
            if (OkButton.Content.ToString() == localization.GetString("Yes"))
            {
                Result = MessageBoxResult.Yes;
            }
            else
            {
                Result = MessageBoxResult.OK;
            }
            Close();
        }

        private void ExtraButton_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }

        public static MessageBoxResult Show(string message, string title = "Warning", MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Warning, string? details = null)
        {
            var window = new WarningWindow(message, title, buttons, icon, details)
            {
                Owner = Application.Current.MainWindow
            };
            window.ShowDialog();
            return window.Result;
        }
    }
}

