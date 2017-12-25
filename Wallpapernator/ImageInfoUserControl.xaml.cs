using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wallpapernator
{
    /// <summary>
    /// Interaction logic for ImageInfoUserControl.xaml
    /// </summary>
    public partial class ImageInfoUserControl : UserControl
    {
        private string imagePath;

        public ImageInfoUserControl(string ImagePath)
        {
            InitializeComponent();
            this.imagePath = ImagePath;

            imgThumb.Source = Helpers.GetBitmapImageThumbFromFile(ImagePath, 142, 80);
            FileInfo fi = new FileInfo(ImagePath);
            lblPath.Content = fi.Name;
            lblDate.Content = fi.CreationTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Test notify window
            //var n = new NotifyWindow(this.imagePath);
            //n.Show();

            if (!File.Exists(this.imagePath)) { return; }

            Process.Start("explorer.exe", this.imagePath);
        }
    }
}
