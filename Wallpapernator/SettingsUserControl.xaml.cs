using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFControls = System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wallpapernator
{
    /// <summary>
    /// Interaction logic for SettingsUserControl.xaml
    /// </summary>
    public partial class SettingsUserControl : WPFControls.UserControl
    {
        private WPSettings wpSettings = new WPSettings();
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
            wpSettings.Save();
            lblSaveSettings.Content = "Settings saved.";
            SettingsUpdatedEvent?.Invoke(this, null);
        }

        private void btnCancelSettings_Click(object sender, RoutedEventArgs e)
        {
            wpSettings.Load();
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
