using System;
using System.Linq;
using System.Windows;
using Forms = System.Windows.Forms;

namespace Wallpapernator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Forms.NotifyIcon notifyIcon;
        private Logger logger = new Logger();
        private SpotlightProcessor spotlight;
        private BingProcessor bing;
        private bool exitMode = false;

        public MainWindow()
        {
            if (Helpers.CheckInstallerStartup())
            {
                exitMode = true;
                this.Close();
                return;
            }

            InitializeComponent();
            InitializeNotifyIcon(); // System tray
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

        #region Notify Icon

        private void InitializeNotifyIcon()
        {
            this.notifyIcon = new Forms.NotifyIcon();
            this.notifyIcon.Click += NotifyIcon_Click;
            this.notifyIcon.Icon = Properties.Resources.icon_ico;
            this.notifyIcon.Visible = true;
            this.notifyIcon.ContextMenu = GetNotifyContextMenu();

            ucSettings.CloseExitEvent += UcSettings_CloseExitEvent;
        }

        private void UcSettings_CloseExitEvent(object sender, string e)
        {
            if (e == "EXIT")
            {
                this.exitMode = true;
                this.Close();
            }
            else
            {
                this.WindowState = WindowState.Minimized;
            }
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private Forms.ContextMenu GetNotifyContextMenu()
        {
            var ctxMenu = new Forms.ContextMenu();
            ctxMenu.MenuItems.Add(new Forms.MenuItem("Open", ctxNotifyOpen_Click));
            ctxMenu.MenuItems.Add(new Forms.MenuItem("Close", ctxNotifyMinimize_Click));
            ctxMenu.MenuItems.Add(new Forms.MenuItem("Exit", ctxNotifyExit_Click));
            return ctxMenu;
        }

        private void ctxNotifyOpen_Click(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private void ctxNotifyMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.Activate();
        }

        private void ctxNotifyExit_Click(object sender, EventArgs e)
        {
            exitMode = true;
            this.Close();
        }

        private void MinimizeApp(bool show)
        {

        }

        private void ToggleMinimize()
        {

        }

        #endregion

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.exitMode)
            {
                e.Cancel = true;
                this.WindowState = WindowState.Minimized;
            }
            else
            {
                this.notifyIcon.Visible = false;
            }
        }
    }
}
