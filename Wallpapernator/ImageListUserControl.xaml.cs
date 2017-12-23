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
        private WPSettings settings = new WPSettings();

        public ImageListUserControl()
        {
            InitializeComponent();
            InitImages();
        }

        public async void InitImages()
        {
            settings.Load();
            await LoadImagesAsync();
        }

        private async Task LoadImagesAsync()
        {
            await this.Dispatcher.InvokeAsync((Action)(() =>
            {
                if (!Directory.Exists(settings.WallpaperPath)) { return; }

                foreach (var img in Directory.GetFiles(settings.WallpaperPath, "*.jpg"))
                {
                    var uc = new ImageInfoUserControl(img);
                    lstImages.Items.Add(uc);
                }
            }));
        }
    }
}
