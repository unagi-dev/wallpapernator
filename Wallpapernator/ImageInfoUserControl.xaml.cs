using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Interaction logic for ImageInfoUserControl.xaml
    /// </summary>
    public partial class ImageInfoUserControl : UserControl
    {
        public ImageInfoUserControl(string ImagePath)
        {
            InitializeComponent();

            var bmi = new BitmapImage();
            bmi.BeginInit();
            bmi.CacheOption = BitmapCacheOption.OnLoad;
            bmi.UriSource = new Uri(ImagePath, UriKind.Absolute);
            bmi.EndInit();
            
            imgThumb.Source = bmi;

            FileInfo fi = new FileInfo(ImagePath);
            lblPath.Content = fi.Name;
            lblDate.Content = fi.CreationTime.ToString("yyyy-MM-dd HH:mm:ss");
            
        }
    }
}
