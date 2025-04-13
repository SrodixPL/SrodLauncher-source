using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.Json;
using System.IO;
using System.Text.RegularExpressions;
using System.Management;
using CmlLib.Core.Auth.Microsoft;

namespace SrodLauncher_v2._0.MVVM.View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        private string username;
        private int ram;
        private double maxSystemRam;
        private readonly Regex usernameRegex = new Regex(@"^[a-zA-Z0-9_]{1,16}$");
        private readonly string settingsPath;

        public SettingsView()
        {
            InitializeComponent();

            settingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SrodLauncherData");

            if (!Directory.Exists(settingsPath))
            {
                Directory.CreateDirectory(settingsPath);
            }

            maxSystemRam = GetTotalPhysicalMemory();
            LoadSettings();

            setUsername.TextChanged += OnUsernameTextChanged;
            setRAM.TextChanged += OnRamTextChanged;
            string versionFilePath = System.IO.Path.Combine(settingsPath, "version.json");

            if (File.Exists(versionFilePath))
            {
                string localVersionJson = File.ReadAllText(versionFilePath);
                var localVersionInfo = JsonSerializer.Deserialize<VersionInfo>(localVersionJson);
                LauncherVersion.Text = localVersionInfo.Version;
                LauncherBuild.Text = localVersionInfo.Build;
                LauncherDate.Text = localVersionInfo.Date;
            }
        }

        private class VersionInfo
        {
            public string Version { get; set; }
            public string Build { get; set; }
            public string Date { get; set; }
        }
        private async void LoadSettings()
        {
            var settings = ReadSettings();

            if (string.IsNullOrEmpty(settings.Username) || !usernameRegex.IsMatch(settings.Username))
            {
                username = "player";
            }
            else
            {
                username = settings.Username;
            }

            if (settings.Ram < 1024 || settings.Ram > maxSystemRam)
            {
                ram = 2048;
            }
            else
            {
                ram = settings.Ram;
            }

            setUsername.Text = username;
            setRAM.Text = ram.ToString();

            // Check if user is already logged in
            try
            {
                var loginHandler = JELoginHandlerBuilder.BuildDefault();
                var session = await loginHandler.AuthenticateSilently(); // Assuming this method exists to check current session

                if (session != null)
                {
                    // User is logged in
                    loggedInStatusText.Text = $"Logged in as: {session.Username}";
                    username = session.Username;
                    setUsername.Text = username;
                    setUsername.IsEnabled = false;

                    // Update login/logout buttons
                    loginButton.Visibility = Visibility.Collapsed;
                    signOutButton.Visibility = Visibility.Visible;

                    SaveSettings();
                }
                else
                {
                    // User is not logged in
                    loggedInStatusText.Text = "Not logged in";
                    setUsername.IsEnabled = true;
                    loginButton.Visibility = Visibility.Visible;
                    signOutButton.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                // Error checking login state, assume not logged in
                loggedInStatusText.Text = "Not logged in";
            }
        }

        private void OnUsernameTextChanged(object sender, TextChangedEventArgs e)
        {
            string newText = setUsername.Text;
            if (usernameRegex.IsMatch(newText))
            {
                setUsername.Foreground = System.Windows.Media.Brushes.Black;
                username = newText;
                SaveSettings();
            }
            else
            {
                setUsername.Foreground = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(255, 100, 100));
            }
        }

        private void OnRamTextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(setRAM.Text, out int ramValue))
            {
                if (ramValue >= 1024 && ramValue <= maxSystemRam)
                {
                    setRAM.Foreground = System.Windows.Media.Brushes.Black;
                    ram = ramValue;
                    SaveSettings();
                }
                else
                {
                    setRAM.Foreground = new System.Windows.Media.SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(255, 100, 100));
                }
            }
            else
            {
                setRAM.Foreground = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(255, 100, 100));
            }
        }

        static ulong GetTotalMemoryInBytes()
        {
            return new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
        }

        static double GetTotalPhysicalMemory()
        {
            ulong totalMemoryInBytes = GetTotalMemoryInBytes();
            return totalMemoryInBytes / (1024.0 * 1024.0);
        }

        private void SaveSettings()
        {
            var settings = new Settings
            {
                Username = username,
                Ram = ram
            };

            try
            {
                var json = JsonSerializer.Serialize(settings);
                string settingsFilePath = Path.Combine(settingsPath, "settings.json");
                File.WriteAllText(settingsFilePath, json);
                MainWindow mainWindow;
                MainWindow.updateWelcomeText.updateText(username);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static Settings ReadSettings()
        {
            string settingsFilePath = Path.Combine(
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

        // First, update your login and signout methods
        public async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var loginHandler = JELoginHandlerBuilder.BuildDefault();
                var session = await loginHandler.Authenticate();

                if (session != null)
                {
                    // Update the UI to show logged in status
                    loggedInStatusText.Text = $"Logged in as: {session.Username}";

                    // Update settings with Microsoft username
                    username = session.Username;
                    setUsername.Text = username;
                    SaveSettings();

                    // Disable username textbox
                    setUsername.IsEnabled = false;

                    // Update login/logout buttons (optional)
                    loginButton.Visibility = Visibility.Collapsed;
                    signOutButton.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login failed: {ex.Message}", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void signOutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var loginHandler = JELoginHandlerBuilder.BuildDefault();
                await loginHandler.Signout();

                // Reset login status
                loggedInStatusText.Text = "Not logged in";

                // Re-enable username textbox
                setUsername.IsEnabled = true;

                // Update login/logout buttons (optional)
                loginButton.Visibility = Visibility.Visible;
                signOutButton.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sign out failed: {ex.Message}", "Sign Out Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class Settings
    {
        public string Username { get; set; } = "player";
        public int Ram { get; set; } = 2048;
    }
}