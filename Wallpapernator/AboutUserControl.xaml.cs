using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for AboutUserControl.xaml
    /// </summary>
    public partial class AboutUserControl : UserControl
    {
        private WPSettings settings = new WPSettings();

        public AboutUserControl()
        {
            InitializeComponent();

            lblVersion.Content = settings.Version;
            btnWpLink.Content = settings.GitUrl;
        }

        private void btnWpLink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(settings.GitUrl);
        }

        private void imgUnagi_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start(settings.UnagiUrl);
        }
    }
}
