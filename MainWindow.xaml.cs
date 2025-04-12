using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
using System.Diagnostics;
using CmlLib.Core;
using DiscordRPC;
using SrodLauncher_v2._0.MVVM.View;

namespace SrodLauncher_v2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly DiscordRpcClient client = new DiscordRpcClient("1359920031231774923");
        SettingsView settingsView;
        public static TextBlock WelcomeText { get; set; }
        private readonly string settingsPath;

        public MainWindow()
        {
            settingsPath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SrodLauncherData");

            if (!Directory.Exists(settingsPath))
            {
                Directory.CreateDirectory(settingsPath);
            }

            InitializeComponent();
            initRPC();
            updateRPC();
            string username = ReadSettings().Username;
            welcomeText.Text = $"Welcome {username}";
            WelcomeText = welcomeText;

            CheckForUpdatesAsync();
        }

        private async void CheckForUpdatesAsync()
        {
            try
            {
                Version localVersion = new Version("0.0.0");
                string versionFilePath = System.IO.Path.Combine(settingsPath, "version.json");

                if (File.Exists(versionFilePath))
                {
                    string localVersionJson = File.ReadAllText(versionFilePath);
                    var localVersionInfo = JsonSerializer.Deserialize<VersionInfo>(localVersionJson);
                    localVersion = new Version(localVersionInfo.Version);
                }

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "SrodLauncher");

                    string remoteVersionUrl = "https://raw.githubusercontent.com/SrodixPL/SrodLauncher/main/version.json";
                    string remoteVersionJson = await httpClient.GetStringAsync(remoteVersionUrl);

                    var remoteVersionInfo = JsonSerializer.Deserialize<VersionInfo>(remoteVersionJson);
                    Version remoteVersion = new Version(remoteVersionInfo.Version);

                    if (remoteVersion != localVersion)
                    {
                        MessageBoxResult result = MessageBox.Show(
                            $"A new version ({remoteVersionInfo.Version}) is available. Would you like to update now?",
                            "Update Available",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Information);

                        if (result == MessageBoxResult.Yes)
                        {
                            try
                            {
                                ProcessStartInfo startInfo = new ProcessStartInfo
                                {
                                    FileName = "Updater.exe",
                                    UseShellExecute = true,
                                    Verb = "runas"
                                };

                                Process.Start(startInfo);

                                Application.Current.Shutdown();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(
                                    $"Failed to launch updater: {ex.Message}",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cannot check for updates: {ex.Message}", "Update Check Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private class VersionInfo
        {
            public string Version { get; set; }
        }

        public static class updateWelcomeText
        {
            public static void updateText(string username)
            {
                WelcomeText.Text = $"Welcome {username}";
            }
        }

        public static void initRPC()
        {
            client.Initialize();
        }

        public static Settings ReadSettings()
        {
            string settingsFilePath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SrodLauncherData",
                "settings.json");

            if (File.Exists(settingsFilePath))
            {
                try
                {
                    var json = File.ReadAllText(settingsFilePath);
                    return JsonSerializer.Deserialize<Settings>(json);
                }
                catch
                {
                    return new Settings { Username = "player", Ram = 2048 };
                }
            }
            else
            {
                return new Settings { Username = "player", Ram = 2048 };
            }
        }

        public static void updateRPC()
        {
            var presence = new RichPresence()
            {
                State = "In Launcher",
            };
            client.SetPresence(presence);
        }

        private void closeProgram(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}