using System;
using System.Drawing;
using System.IO;
using System.Timers;

namespace Wallpapernator
{
    public class SpotlightProcessor
    {
        //private FileSystemWatcher watcher;
        private Timer spotlightTimer;
        private string watchPath = string.Empty;
        private string destPath = string.Empty;
        private int imgWidth;
        private int imgHeight;
        private int interval = 30 * 60 * 1000; // 30 minutes

        public event EventHandler<string> ImageAddedEvent;
        public event EventHandler<string> ErrorEvent;

        public SpotlightProcessor(string WatchPath, string DestinationPath, int ImageWidth, int ImageHeight)
        {
            this.watchPath = WatchPath;
            this.destPath = DestinationPath;
            this.imgWidth = ImageWidth;
            this.imgHeight = ImageHeight;
        }

        public void Start()
        {
            if (!Directory.Exists(this.watchPath) || !Directory.Exists(this.destPath)) { return; }

            this.Stop();
            ScanSpotlightImages();
            spotlightTimer = new Timer(interval);
            spotlightTimer.Elapsed += SpotlightTimer_Elapsed; ;
            spotlightTimer.Start();
            //watcher = new FileSystemWatcher(this.watchPath, "*.*");
            //watcher.Changed += Watcher_Changed;
            //watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            if (spotlightTimer == null || !spotlightTimer.Enabled) { return; }

            spotlightTimer.Stop();
            spotlightTimer.Dispose();
        }

        private void SpotlightTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ScanSpotlightImages();
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (e.ChangeType != WatcherChangeTypes.Created && e.ChangeType != WatcherChangeTypes.Changed) { return; }

                var fullPath = e.FullPath;
                var fileName = e.Name;

                try
                {
                    using (var bmp = Bitmap.FromFile(fullPath))
                    {
                        if (bmp.Width == this.imgWidth && bmp.Height == this.imgHeight)
                        {
                            var destFile = Path.Combine(this.destPath, $"{fileName}.jpg");
                            if (!File.Exists(destFile))
                            {
                                File.Copy(fullPath, destFile);
                                this.ImageAddedEvent?.Invoke(this, destFile);
                            }
                        }
                    }
                }
                catch { }

            }
            catch (Exception ex) { this.ErrorEvent?.Invoke(this, ex.Message); }
        }

        private void ScanSpotlightImages()
        {
            try
            {
                foreach (var fullPath in Directory.GetFiles(this.watchPath))
                {
                    var fi = new FileInfo(fullPath);
                    var destFile = Path.Combine(this.destPath, $"{fi.Name}.jpg");
                    if (File.Exists(destFile)) { continue; }

                    using (var bmp = Bitmap.FromFile(fullPath))
                    {
                        if (bmp.Width == this.imgWidth && bmp.Height == this.imgHeight)
                        {
                            File.Copy(fullPath, destFile);
                            this.ImageAddedEvent?.Invoke(this, destFile);
                        }
                    }
                }
            }
            catch (Exception ex) { this.ErrorEvent?.Invoke(this, ex.Message); }
        }
    }
}
