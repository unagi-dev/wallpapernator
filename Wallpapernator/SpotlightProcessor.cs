using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallpapernator
{
    public class SpotlightProcessor: IDisposable
    {
        private FileSystemWatcher watcher;
        private string watchPath = string.Empty;
        private string destPath = string.Empty;
        private int imgWidth;
        private int imgHeight;

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

            WatcherPathInitialScan();
            watcher = new FileSystemWatcher(this.watchPath, "*.*");
            watcher.Changed += Watcher_Changed;
            watcher.EnableRaisingEvents = true;
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

        private void WatcherPathInitialScan()
        {
            try
            {
                foreach (var fullPath in Directory.GetFiles(this.watchPath))
                {
                    using (var bmp = Bitmap.FromFile(fullPath))
                    {
                        if (bmp.Width == this.imgWidth && bmp.Height == this.imgHeight)
                        {
                            var fi = new FileInfo(fullPath);
                            var destFile = Path.Combine(this.destPath, $"{fi.Name}.jpg");

                            if (!File.Exists(destFile))
                            {
                                File.Copy(fullPath, destFile);
                                this.ImageAddedEvent?.Invoke(this, destFile);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { this.ErrorEvent?.Invoke(this, ex.Message); }
        }

        public void Dispose()
        {
            if (watcher != null) { watcher.Dispose(); }
        }
    }
}
