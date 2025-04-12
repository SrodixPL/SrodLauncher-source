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
using CmlLib.Core;
using CmlLib.Core.Version;
using SrodLauncher_v2._0.Core;

namespace SrodLauncher_v2._0.MVVM.View
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MinecraftView : UserControl
    {
        private LaunchMC launchMC;
        private SettingsView SettingsView;
        private bool isLoaded = false;

        public MinecraftView()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                if (!isLoaded)
                {
                    loadVers();
                    isLoaded = true;
                    
                }
            };
        }

        private async void loadVers()
        {
            Cursor = Cursors.Wait;
            var launcher = new MinecraftLauncher();
            var versions = await launcher.GetAllVersionsAsync();
            isLoaded = true;

            foreach (var version in versions)
            {
                if(version.Type == "release")
                {
                    versionSelect.Items.Add(version);
                }
            }
            Cursor = Cursors.Arrow;
        }

        public string GetVersion()
        {
            string version = versionSelect.Text;
            version = version.Replace("release ", "");
            return version;
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
            if (versionSelect.SelectedItem == null)
            {
                MessageBox.Show("Please select a version", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            playButton.Visibility = Visibility.Hidden;
            versionSelect.Visibility = Visibility.Hidden;
            loadingText.Visibility = Visibility.Visible;
            launchMC = new LaunchMC(SettingsView, this);
        }
    }
}
