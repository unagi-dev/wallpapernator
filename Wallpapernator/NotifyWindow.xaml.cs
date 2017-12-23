using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Wallpapernator
{
    /// <summary>
    /// Interaction logic for NotifyWindow.xaml
    /// </summary>
    public partial class NotifyWindow : Window
    {
        private int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
        private int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;

        public NotifyWindow(string ImagePath)
        {
            InitializeComponent();

            this.Left = screenWidth - this.Width;
            this.Top = screenHeight - this.Height;

            var bmi = new BitmapImage();
            bmi.BeginInit();
            bmi.CacheOption = BitmapCacheOption.OnLoad;
            bmi.UriSource = new Uri(ImagePath, UriKind.Absolute);
            bmi.EndInit();
            
            imgBackground.Source = bmi;

            //Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            //{
            //    var workingArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            //    var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
            //    var corner = transform.Transform(new Point(workingArea.Right, workingArea.Bottom));

            //    this.Left = corner.X - this.ActualWidth;
            //    this.Top = corner.Y - this.ActualHeight;
            //}));

            InitializeAnimation();
        }

        private void InitializeAnimation()
        {
            var slideInAnimation = new DoubleAnimation(screenWidth, this.Left, new Duration(TimeSpan.FromMilliseconds(300)));
            Storyboard.SetTargetProperty(slideInAnimation, new PropertyPath("Left"));

            var fadeOutAnimation = new DoubleAnimationUsingKeyFrames();
            fadeOutAnimation.KeyFrames.Add(new SplineDoubleKeyFrame(1, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(3))));
            fadeOutAnimation.KeyFrames.Add(new SplineDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(6))));
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("Opacity"));

            var windowStoryboard = new Storyboard();
            windowStoryboard.Completed += WindowStoryboard_Completed;
            windowStoryboard.Children.Add(slideInAnimation);
            windowStoryboard.Children.Add(fadeOutAnimation);

            var windowEventTrigger = new EventTrigger();
            var windowBeginStoryboard = new BeginStoryboard();
            windowEventTrigger.RoutedEvent = Window.LoadedEvent;
            windowBeginStoryboard.Storyboard = windowStoryboard;
            windowEventTrigger.Actions.Add(windowBeginStoryboard);
            WallpaperNotification.Triggers.Add(windowEventTrigger);
        }

        private void WindowStoryboard_Completed(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
