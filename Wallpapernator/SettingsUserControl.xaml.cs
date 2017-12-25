using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using WPFControls = System.Windows.Controls;

namespace Wallpapernator
{
    /// <summary>
    /// Interaction logic for SettingsUserControl.xaml
    /// </summary>
    public partial class SettingsUserControl : WPFControls.UserControl
    {
        private WPSettings wpSettings = new WPSettings();
        private Brush successBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6495ED"));
        private Brush errorBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFED6464"));

        public event EventHandler SettingsUpdatedEvent;

        public WPSettings Wps
        {
            get
            {
                return wpSettings;
            }
        }

        public SettingsUserControl()
        {
            InitializeComponent();
            this.DataContext = wpSettings;
        }

        private void btnBrowseWallpaper_Click(object sender, RoutedEventArgs e)
        {
            var path = BrowseFolder();
            if (!string.IsNullOrEmpty(path))
            {
                this.wpSettings.WallpaperPath = path;
            }
        }

        private void btnBrowseSpotlight_Click(object sender, RoutedEventArgs e)
        {
            var path = BrowseFolder();
            if (!string.IsNullOrEmpty(path))
            {
                this.wpSettings.SpotlightPath = path;
            }
        }

        private void btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            ClearSaveMessage();

            if (!validateSettings()) { return; }

            wpSettings.Save();
            SettingsUpdatedEvent?.Invoke(this, null);

            lblSaveSettings.Content = "Settings saved";
            lblSaveSettings.Foreground = successBrush;
            Helpers.AnimationFadeInOut(lblSaveSettings, 300, 2000, 800);
        }

        private void btnCancelSettings_Click(object sender, RoutedEventArgs e)
        {
            ClearSaveMessage();
            wpSettings.Load();
        }

        private void ClearSaveMessage()
        {
            lblSaveSettings.Opacity = 0;
            lblSaveSettings.Content = "";
        }

        private string BrowseFolder()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK ||
                    !Directory.Exists(dialog.SelectedPath))
                {
                    return null;
                }

                return dialog.SelectedPath;
            };
        }

        private bool validateSettings()
        {
            var msg = string.Empty;
            var res = 0;

            if (!string.IsNullOrEmpty(wpSettings.WallpaperPath) && !Directory.Exists(wpSettings.WallpaperPath))
            {
                msg = "Error: Invalid wallpaper directory.";
            }
            else if (!string.IsNullOrEmpty(wpSettings.SpotlightPath) && !Directory.Exists(wpSettings.SpotlightPath))
            {
                msg = "Error: Invalid spotlight directory.";
            }
            else if (!int.TryParse(txtImgWidth.Text, out res) || !int.TryParse(txtImgHeight.Text, out res))
            {
                msg = "Error: Invalid image size.";
            }

            if (string.IsNullOrEmpty(msg)) { return true; }

            lblSaveSettings.Content = msg;
            lblSaveSettings.Foreground = errorBrush;
            Helpers.AnimationFadeIn(lblSaveSettings, 300);

            return false;
        }

        #region Data

        public KeyValuePair<int, string>[] BingIntervalList
        {
            get
            {
                return bingIntervalList;
            }
        }

        public KeyValuePair<int, string>[] bingIntervalList = {
            new KeyValuePair<int, string>(0, "Never"),
            new KeyValuePair<int, string>(1, "Every 1 hour"),
            new KeyValuePair<int, string>(3, "Every 3 hours"),
            new KeyValuePair<int, string>(6, "Every 6 hours"),
            new KeyValuePair<int, string>(12, "Every 12 hours")
        };

        #endregion

    }
}
