using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Wallpapernator
{
    public static class Helpers
    {

        public static BitmapImage GetBitmapImageThumbFromFile(string file, int width, int height)
        {
            using (Bitmap bmp = new Bitmap(file))
            using (Image thm = bmp.GetThumbnailImage(width, height, null, IntPtr.Zero))
            using (var stream = new MemoryStream())
            {
                thm.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                stream.Position = 0;

                var bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.StreamSource = stream;
                bmi.EndInit();
                bmi.Freeze();

                return bmi;
            }
        }

    }
}
