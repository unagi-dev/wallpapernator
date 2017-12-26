using System;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wallpapernator
{
    /// <summary>
    /// Interaction logic for AboutUserControl.xaml
    /// </summary>
    public partial class AboutUserControl : UserControl
    {
        private WPSettings settings = new WPSettings();
        private Regex rxVer = new Regex(@"<title>Release v(\d+\.\d+\.\d+)", RegexOptions.IgnoreCase);

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
                verClient.DownloadStringAsync(new Uri(settings.LatestReleaseUrl));
            }
        }

        private void VerClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var msg = "Up to date.";
            var hasUpdate = false;

            if (e.Error != null)
            {
                msg = "Error getting version: " + e.Error.Message;
            }
            else
            {
                var match = rxVer.Match(e.Result);

                if (!match.Success || match.Groups.Count < 2)
                {
                    msg = "Error finding version info.";
                }
                else
                {
                    var latest = match.Groups[1].Value;
                    if (settings.VersionShort != latest)
                    {
                        msg = $"New version available: {latest}";
                        hasUpdate = true;
                    }
                }
            }

            lblUpdateInfo.Content = msg;
            Helpers.AnimationFadeOut(ucSpinner, 100);
            Helpers.AnimationFadeInOut(lblUpdateInfo, 300, 3000, 5000);
            if (hasUpdate)
            {
                btnLatestRelease.Visibility = Visibility.Visible;
                Helpers.AnimationFadeInOut(btnLatestRelease, 300, 3000, 5000, new EventHandler(btnLatestRelease_AnimationCompleted));
            }
        }

        private void btnLatestRelease_AnimationCompleted(object sender, EventArgs e)
        {
            btnLatestRelease.Visibility = Visibility.Hidden;
        }

        private void btnLatestRelease_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(settings.LatestReleaseUrl);
        }
    }
}
