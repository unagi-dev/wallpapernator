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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wallpapernator
{
    /// <summary>
    /// Interaction logic for ImageListUserControl.xaml
    /// </summary>
    public partial class ImageListUserControl : UserControl
    {
        private string wpPath;

        public ImageListUserControl()
        {
            InitializeComponent();

            this.getPath();
            InitImages();
        }

        // Returns true if path has changed
        private bool getPath()
        {
            var settingsPath = (new WPSettings()).WallpaperPath;

            if (string.IsNullOrEmpty(this.wpPath) || string.Compare(settingsPath, this.wpPath, true) != 0)
            {
                this.wpPath = settingsPath;
                return true;
            }

            return false;
        }

        private async void InitImages()
        {
            await LoadImagesAsync();
        }

        public async void Reload()
        {
            if (!getPath()) { return; }

            lstImages.Items.Clear();
            await this.LoadImagesAsync();
        }

        private async Task LoadImagesAsync()
        {
            if (!Directory.Exists(this.wpPath)) { return; }

            await this.Dispatcher.InvokeAsync((Action)(() =>
            {
                foreach (var img in Directory.GetFiles(this.wpPath, "*.jpg"))
                {
                    this.AddImage(img);
                }
                GC.Collect();
            }));
        }

        public void AddImage(string path)
        {
            if (!File.Exists(path)) { return; }

            this.Dispatcher.Invoke((Action)(() =>
            {
                var uc = new ImageInfoUserControl(path);
                lstImages.Items.Add(uc);
            }));
        }
    }
}
