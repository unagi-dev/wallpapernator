using IWshRuntimeLibrary;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Wallpapernator
{
    public class WPSettings : INotifyPropertyChanged
    {
        private Properties.Settings props = Properties.Settings.Default;
        private string wallpaperPath = string.Empty;
        private string spotlightPath = string.Empty;
        private int imageWidth;
        private int imageHeight;
        private int bingIntervalHours;
        private bool runAtStartup;
        private string version;
        private string gitUrl = "https://github.com/unagi-dev/wallpapernator";
        private string unagiUrl = "https://github.com/unagi-dev";
        private string latestReleaseUrl = "https://github.com/unagi-dev/wallpapernator/releases/latest";

        private string spotlightDir = @"Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets";
        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public WPSettings()
        {
            this.Load();
            this.FirstRun();
        }

        public void Load()
        {
            UpdateSettings();

            this.version = System.Windows.Forms.Application.ProductVersion;
            this.WallpaperPath = props.WallpaperPath;
            this.SpotlightPath = GetSpotlightPath();
            this.ImageWidth = props.ImageWidth;
            this.ImageHeight = props.ImageHeight;
            this.bingIntervalHours = props.BingIntervalHours;
            this.RunAtStartup = props.RunAtStartup;
        }

        private string GetSpotlightPath()
        {
            if (!string.IsNullOrWhiteSpace(props.SpotlightPath)) { return props.SpotlightPath; }

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), this.spotlightDir);

            if (!Directory.Exists(path)) { return string.Empty; }

            props.SpotlightPath = path;
            props.Save();
            return path;
        }

        public void Save()
        {
            this.CheckChanges();

            props.WallpaperPath = this.wallpaperPath;
            props.SpotlightPath = this.spotlightPath;
            props.ImageWidth = this.imageWidth;
            props.ImageHeight = this.imageHeight;
            props.BingIntervalHours = this.bingIntervalHours;
            props.RunAtStartup = this.runAtStartup;

            props.Save();
        }

        private void UpdateSettings()
        {
            // Copy user settings from previous application version if necessary
            if (props.UpdateSettings)
            {
                props.Upgrade();
                props.UpdateSettings = false;
                props.Save();
            }
        }

        private void CheckChanges()
        {
            if (props.RunAtStartup != this.runAtStartup)
            {
                SetRunAtStartup(this.runAtStartup);
            }
        }

        private void FirstRun()
        {
            if (!props.FirstRun) { return; }

            SetRunAtStartup(props.RunAtStartup);
            props.FirstRun = false;
            props.Save();
        }

        private void SetRunAtStartup(bool doit)
        {
            var myPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            var startupDir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var shortcutAddress = Path.Combine(startupDir, "Wallpapernator.lnk");

            if (!doit && System.IO.File.Exists(shortcutAddress))
            {
                System.IO.File.Delete(shortcutAddress);
                return;
            }

            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "Wallpapernator";
            shortcut.TargetPath = myPath;
            shortcut.Save();
            shell = null;
        }

        public string WallpaperPath
        {
            get
            {
                return this.wallpaperPath;
            }

            set
            {
                if (value != this.wallpaperPath)
                {
                    this.wallpaperPath = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string SpotlightPath
        {
            get
            {
                return this.spotlightPath;
            }

            set
            {
                if (value != this.spotlightPath)
                {
                    this.spotlightPath = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int ImageWidth
        {
            get
            {
                return this.imageWidth;
            }

            set
            {
                if (value != this.imageWidth)
                {
                    this.imageWidth = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int ImageHeight
        {
            get
            {
                return this.imageHeight;
            }

            set
            {
                if (value != this.imageHeight)
                {
                    this.imageHeight = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int BingIntervalHours
        {
            get
            {
                return this.bingIntervalHours;
            }

            set
            {
                if (value != this.bingIntervalHours)
                {
                    this.bingIntervalHours = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool RunAtStartup
        {
            get
            {
                return this.runAtStartup;
            }

            set
            {
                if (value != this.runAtStartup)
                {
                    this.runAtStartup = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Version
        {
            get
            {
                return this.version;
            }
        }

        public string VersionShort
        {
            get
            {
                return this.version.Substring(0, this.version.LastIndexOf('.'));
            }
        }

        public string GitUrl
        {
            get
            {
                return this.gitUrl;
            }
        }

        public string UnagiUrl
        {
            get
            {
                return this.unagiUrl;
            }
        }

        public string LatestReleaseUrl
        {
            get
            {
                return this.latestReleaseUrl;
            }
        }
    }
}
