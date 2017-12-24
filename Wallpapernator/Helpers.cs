using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Controls = System.Windows.Controls;
using System.Windows.Media.Animation;
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

        public static void AnimationFadeInOut(Controls.ContentControl element, int fadeInMs = 500, int fadeOutMs = 500, int fadeOutStartMs = 1000)
        {
            var storyboard = new Storyboard();

            var fadeInAnimation = new DoubleAnimation()
            { From = 0.0, To = 1.0, Duration = new Duration(TimeSpan.FromMilliseconds(fadeInMs)) };

            var fadeOutAnimation = new DoubleAnimation()
            { From = 1.0, To = 0.0, Duration = new Duration(TimeSpan.FromMilliseconds(fadeOutMs)) };
            fadeOutAnimation.BeginTime = TimeSpan.FromMilliseconds(fadeOutStartMs);

            Storyboard.SetTargetName(fadeInAnimation, element.Name);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity", 1));
            storyboard.Children.Add(fadeInAnimation);
            storyboard.Begin(element);

            Storyboard.SetTargetName(fadeOutAnimation, element.Name);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("Opacity", 0));
            storyboard.Children.Add(fadeOutAnimation);
            storyboard.Begin(element);
        }

        public static void AnimationFadeIn(Controls.ContentControl element, int fadeInMs = 500)
        {
            var storyboard = new Storyboard();

            var fadeInAnimation = new DoubleAnimation()
            { From = 0.0, To = 1.0, Duration = new Duration(TimeSpan.FromMilliseconds(fadeInMs)) };

            Storyboard.SetTargetName(fadeInAnimation, element.Name);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity", 1));
            storyboard.Children.Add(fadeInAnimation);
            storyboard.Begin(element);
        }

    }
}
