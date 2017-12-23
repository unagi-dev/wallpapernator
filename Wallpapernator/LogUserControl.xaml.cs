using System;
using System.Collections.Generic;
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
