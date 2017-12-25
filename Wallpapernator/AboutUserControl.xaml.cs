using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wallpapernator
{
    /// <summary>
    /// Interaction logic for AboutUserControl.xaml
    /// </summary>
    public partial class AboutUserControl : UserControl
    {
        private WPSettings settings = new WPSettings();

        public AboutUserControl()
        {
            InitializeComponent();

            lblVersion.Content = settings.Version;
            btnWpLink.Content = settings.GitUrl;
        }

        private void btnWpLink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(settings.GitUrl);
        }

        private void imgUnagi_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start(settings.UnagiUrl);
        }

        private async void btnCheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            lblUpdateInfo.Content = string.Empty;
            lblUpdateInfo.Opacity = 0;
            Helpers.AnimationFadeIn(ucSpinner, 300);
            await Task.Delay(1000);
            var info = new string[] { };
            var verString = string.Empty;

            using (var verClient = new WebClient())
            {
                verClient.DownloadStringCompleted += VerClient_DownloadStringCompleted;
                verClient.DownloadStringAsync(new Uri(settings.VersionCheckUrl));
            }
        }

        private void VerClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var msg = string.Empty;

            if (e.Error != null)
            {
                msg = "Error getting version: " + e.Error.Message;
            }
            else {
                var data = e.Result.Split('\n');

                if (data[0] == settings.Version)
                {
                    msg = "Up to date.";
                }
                else {
                    msg = $"New version available: {data[0]} ({data[1]})";
                }
            }

            lblUpdateInfo.Content = msg;
            Helpers.AnimationFadeOut(ucSpinner, 100);
            Helpers.AnimationFadeInOut(lblUpdateInfo, 300, 3000, 5000);
        }
    }
}
