using System;
using System.Windows;
using System.Windows.Controls;

namespace Wallpapernator
{
    /// <summary>
    /// Interaction logic for LogUserControl.xaml
    /// </summary>
    public partial class LogUserControl : UserControl
    {
        public event EventHandler ClearLogEvent;
        public event EventHandler OpenLocationEvent;

        public LogUserControl()
        {
            InitializeComponent();
        }

        public void Append(string text)
        {
            txtLog.Text += text;
            LogScrollViewer.ScrollToBottom();
        }

        private void mnuLogClear_Click(object sender, RoutedEventArgs e)
        {
            txtLog.Text = "";
            ClearLogEvent?.Invoke(this, null);
        }

        private void mnuOpenLocation_Click(object sender, RoutedEventArgs e)
        {
            OpenLocationEvent?.Invoke(this, null);
        }

    }
}
