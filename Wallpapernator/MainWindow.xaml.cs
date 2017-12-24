using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Logger logger = new Logger();
        private SpotlightProcessor spotlight;
        private BingProcessor bing;

        public MainWindow()
        {
            InitializeComponent();
            this.Title += " v" + ucSettings.Wps.VersionShort;
            InitLogger();
            InitService();
        }

        private void InitService()
        {
            InitSpotlight();
            InitBing();
        }

        private void ucSettings_SettingsUpdatedEvent(object sender, EventArgs e)
        {
            InitService();
            ucImageList.Reload();
            logger.Log($"Settings updated.");
        }

        #region Spotlight

        private void InitSpotlight()
        {
            if (spotlight != null) { spotlight.Dispose(); }

            spotlight = new SpotlightProcessor(ucSettings.Wps.SpotlightPath, ucSettings.Wps.WallpaperPath, ucSettings.Wps.ImageWidth, ucSettings.Wps.ImageHeight);
            spotlight.ImageAddedEvent += Spotlight_ImageAddedEvent;
            spotlight.ErrorEvent += Spotlight_ErrorEvent;
            spotlight.Start();
        }

        private void Spotlight_ImageAddedEvent(object sender, string e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                logger.Log("Spotlight image added: " + e);
                var n = new NotifyWindow(e);
                n.Show();
                ucImageList.AddImage(e);
            }));
        }

        private void Spotlight_ErrorEvent(object sender, string e)
        {
            logger.Log($"Error: {e}");
        }

        #endregion

        #region Bing

        private void InitBing()
        {
            if (ucSettings.Wps.BingIntervalHours < 1) { return; }

            bing = new BingProcessor(ucSettings.Wps.WallpaperPath, ucSettings.Wps.BingIntervalHours);
            bing.ImageAddedEvent += Bing_ImageAddedEvent;
            bing.ErrorEvent += Bing_ErrorEvent;
            bing.Start();
        }

        private void Bing_ImageAddedEvent(object sender, string e)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                logger.Log("Bing image added: " + e);
                var n = new NotifyWindow(e);
                n.Show();
                ucImageList.AddImage(e);
            }));
        }

        private void Bing_ErrorEvent(object sender, string e)
        {
            logger.Log($"Error: {e}");
        }

        #endregion

        #region Logger

        private void InitLogger()
        {
            ucLog.Append(logger.GetLog());
            logger.MessageEvent += Logger_MessageEvent;
            logger.Log("App started.");
        }

        private void Logger_MessageEvent(object sender, string e)
        {
            ucLog.Append(e);
        }

        private void ucLog_ClearLogEvent(object sender, EventArgs e)
        {
            logger.Clear();
        }

        private void ucLog_OpenLocationEvent(object sender, EventArgs e)
        {
            logger.OpenLocation();
        }

        #endregion

    }
}
