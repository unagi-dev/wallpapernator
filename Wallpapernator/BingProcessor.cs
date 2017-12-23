using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace Wallpapernator
{
    public class BingProcessor
    {
        private static string bingUrl = "https://www.bing.com";
        private string destPath = string.Empty;
        private int intervalHours;
        private Timer bingTimer;

        public event EventHandler<string> ImageAddedEvent;
        public event EventHandler<string> ErrorEvent;

        public BingProcessor(string DestinationPath, int IntervalHours)
        {
            this.destPath = DestinationPath;
            this.intervalHours = IntervalHours;
        }

        public async void Start()
        {
            this.Stop();
            await GetBingImageAsync();
            bingTimer = new Timer(intervalHours * 60 * 60 * 1000);
            bingTimer.Elapsed += BingTimer_Elapsed;
            bingTimer.Start();
        }

        public void Stop()
        {
            if (bingTimer == null || !bingTimer.Enabled) { return; }

            bingTimer.Stop();
            bingTimer.Dispose();
        }

        private async void BingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await GetBingImageAsync();
        }

        public async Task GetBingImageAsync()
        {
            await Task.Run(() => GetBingImage());
        }

        private void GetBingImage()
        {
            try
            {
                var html = string.Empty;
                var imgUrl = string.Empty;
                html = getHtml();
                imgUrl = getImageUrl(html);
                downloadImage(imgUrl);
            }
            catch (Exception ex)
            {
                this.ErrorEvent?.Invoke(this, ex.Message);
            }
        }

        private string getHtml()
        {
            var html = string.Empty;

            var request = (HttpWebRequest)WebRequest.Create(bingUrl);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            return html;
        }

        private string getImageUrl(string html)
        {
            var imgUrl = string.Empty;

            var rx = new Regex(@"(\/([\w\d]+))+\.jpg");

            var m = rx.Match(html);

            if (m.Success)
            {
                imgUrl = m.Value;
            }

            return bingUrl + imgUrl;
        }

        private void downloadImage(string imgUrl)
        {
            var filename = imgUrl.Substring(imgUrl.LastIndexOf('/') + 1);
            var destFile = Path.Combine(destPath, filename);

            if (File.Exists(destFile)) { return; }

            using (var wc = new WebClient())
            {
                wc.DownloadFile(imgUrl, destFile);
            }

            this.ImageAddedEvent?.Invoke(this, destFile);
        }

    }
}
