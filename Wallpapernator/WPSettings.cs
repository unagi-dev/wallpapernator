using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Wallpapernator
{
    public class WPSettings : INotifyPropertyChanged
    {
        private string wallpaperPath = string.Empty;
        private string spotlightPath = string.Empty;
        private int imageWidth;
        private int imageHeight;
        private int bingIntervalHours;
        private bool runAtStartup;

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
        }

        public void Load()
        {
            this.WallpaperPath = Properties.Settings.Default["WallpaperPath"].ToString();
            this.SpotlightPath = Properties.Settings.Default["SpotlightPath"].ToString();
            this.ImageWidth = (int)Properties.Settings.Default["ImageWidth"];
            this.ImageHeight = (int)Properties.Settings.Default["ImageHeight"];
            this.bingIntervalHours = (int)Properties.Settings.Default["BingIntervalHours"];
            this.RunAtStartup = (bool)Properties.Settings.Default["RunAtStartup"];
        }

        public void Save()
        {
            Properties.Settings.Default["WallpaperPath"] = this.wallpaperPath;
            Properties.Settings.Default["SpotlightPath"] = this.spotlightPath;
            Properties.Settings.Default["ImageWidth"] = this.imageWidth;
            Properties.Settings.Default["ImageHeight"] = this.imageHeight;
            Properties.Settings.Default["BingIntervalHours"] = this.bingIntervalHours;
            Properties.Settings.Default["RunAtStartup"] = this.runAtStartup;

            Properties.Settings.Default.Save();
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

    }
}
