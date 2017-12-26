using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
        private Dictionary<string, UserControl> userControls = new Dictionary<string, UserControl>();

        public MainWindow()
        {
            if (Helpers.CheckInstallerStartup())
            {
                exitMode = true;
                this.Close();
                return;
            }

            InitializeComponent();
            
            InitService();
        }

        private void InitService()
        {
            InitializeNotifyIcon(); // System tray
            this.Title += " v" + ucSettings.Wps.VersionShort;

            InitLogger();
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
            Activate();
            ShowForm(true);
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
            ShowForm(false);
        }

        private void ctxNotifyExit_Click(object sender, EventArgs e)
        {
            exitMode = true;
            this.Close();
        }

        #endregion

        #region Window misc

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            userControls.Add("Settings", ucSettings);
            userControls.Add("Images", ucImageList);
            userControls.Add("Log", ucLog);
            userControls.Add("About", ucAbout);

            Panel.SetZIndex(ucSettings, 9);
            Helpers.AnimationFadeIn(ucSettings, 300);
        }

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

        // Enable window drag on imitation toolbar
        private void grdToolbar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (System.Windows.Input.Mouse.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                DragMove();
        }

        private void btnClose_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ShowForm(false);
        }
        
        private void ToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            var itm = ((Button)sender).Content as string;

            foreach (var uc in userControls.Where(x => x.Key != itm))
            {
                if (uc.Value.Opacity == 0) { continue; }
                Helpers.AnimationFadeOut(uc.Value, 250);
                Panel.SetZIndex(uc.Value, 0);
            }

            Panel.SetZIndex(userControls[itm], 9);
            Helpers.AnimationFadeIn(userControls[itm], 250);
        }

        private void ToolbarButtonExit_Click(object sender, RoutedEventArgs e)
        {
            exitMode = true;
            this.Close();
        }

        private void ShowForm(bool show)
        {
            if (show) { this.Activate(); }

            if (show && mainWindow.Opacity == 0)
            {
                mainWindow.WindowState = WindowState.Normal;
                Helpers.AnimationFadeIn(mainWindow, 250);
                this.ShowInTaskbar = true;
            }
            else if (!show && mainWindow.Opacity == 1)
            {
                Helpers.AnimationFadeOut(mainWindow, 250);
                this.ShowInTaskbar = false;
                mainWindow.WindowState = WindowState.Minimized;
            }
        }

        #endregion
        
    }
}
