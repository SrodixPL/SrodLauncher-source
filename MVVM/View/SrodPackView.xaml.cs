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
using System.IO;
using System.Text.Json;
using System.Security.Cryptography.X509Certificates;
using SrodLauncher_v2._0.Core;

namespace SrodLauncher_v2._0.MVVM.View
{
    /// <summary>
    /// Interaction logic for SrodPackView.xaml
    /// </summary>
    public partial class SrodPackView : UserControl
    {
        private LaunchSP launchSP;
        private SettingsView SettingsView;

        public SrodPackView()
        {
            InitializeComponent();
        }

        public Button GetPlayButton()
        {
            return playButton;
        }

        public TextBlock GetLauncherText()
        {
            return loadingText;
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            playButton.Visibility = Visibility.Hidden;
            loadingText.Visibility = Visibility.Visible;
            launchSP = new LaunchSP(SettingsView, this);
        }
    }
}
