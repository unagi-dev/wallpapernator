using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallpapernator
{
    public class Logger
    {
        public event EventHandler<string> MessageEvent;
        private string LOG_FILE = "logfile.txt";

        public void Log(string message)
        {
            string msg = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {message}{Environment.NewLine}";

            // Log to file
            WriteLog(msg);

            // Raise event
            MessageEvent?.Invoke(this, msg);
        }

        private void WriteLog(string msg)
        {
            File.AppendAllText(LOG_FILE, msg);
        }

        public string GetLog()
        {
            if (!File.Exists(LOG_FILE)) { return ""; }

            return File.ReadAllText(LOG_FILE);
        }

        public void Clear()
        {
            if (!File.Exists(LOG_FILE)) { return; }

            File.WriteAllText(LOG_FILE, "");
        }

        public void OpenLocation()
        {
            if (!File.Exists(LOG_FILE)) { return; }

            var fi = new FileInfo(LOG_FILE);
            Process.Start("explorer.exe", fi.DirectoryName);
        }
    }
}
