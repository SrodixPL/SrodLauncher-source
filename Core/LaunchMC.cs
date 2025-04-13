using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Controls;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.ProcessBuilder;
using DiscordRPC;
using SrodLauncher_v2._0.MVVM.View;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
namespace SrodLauncher_v2._0.Core
{
    internal class LaunchMC
    {
        public class SettingsReader
        {
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
        }
        private string username;
        private int ram;
        private string version;
        private TextBlock launcherText;

        public LaunchMC(SettingsView settingsView, MinecraftView minecraftView)
        {
            var settings = SettingsReader.ReadSettings();
            username = settings.Username;
            ram = settings.Ram;
            version = minecraftView.GetVersion();
            launcherText = minecraftView.GetLauncherText();
            System.Windows.Controls.Button playButton = minecraftView.GetPlayButton();
            playButton.IsEnabled = true;

            async void startGame()
            {
                try
                {
                    playButton.IsEnabled = false;
                    launcherText.Text = "Preparing to launch...";

                    var path = new MinecraftPath(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "SrodLauncherData",
                        ".minecraft"));

                    var launcher = new MinecraftLauncher(path);
                    launcher.ByteProgressChanged += (sender, args) =>
                    {
                        var downloaded = args.ProgressedBytes / (1024.0 * 1024.0);
                        var total = args.TotalBytes / (1024.0 * 1024.0);
                        launcherText.Text = $"Downloading {downloaded:0.00} MB of {total:0.00} MB";
                    };

                    await launcher.InstallAsync(version);

                    MSession session;
                    bool isLoggedIn = false;

                    try
                    {
                        var loginHandler = JELoginHandlerBuilder.BuildDefault();
                        var msSession = await loginHandler.AuthenticateSilently();

                        if (msSession != null)
                        {
                            isLoggedIn = true;
                            session = msSession;
                            launcherText.Text = $"Launching as {msSession.Username} (Premium)...";
                        }
                        else
                        {
                            session = MSession.CreateOfflineSession(username);
                            launcherText.Text = $"Launching as {username} (Offline)...";
                        }
                    }
                    catch (Exception ex)
                    {
                        session = MSession.CreateOfflineSession(username);
                        launcherText.Text = $"Authentication error, launching in offline mode as {username}...";
                    }

                    var launchOption = new MLaunchOption
                    {
                        Session = session,
                        MaximumRamMb = ram,
                        ScreenWidth = 1200,
                        ScreenHeight = 720,
                        FullScreen = false,
                        Path = path,
                    };

                    launcherText.Text = isLoggedIn ?
                        "Launching Minecraft with premium account..." :
                        "Launching Minecraft in offline mode...";

                    var process = await launcher.CreateProcessAsync(version, launchOption);
                    process.Start();
                    launcherText.Text = "Minecraft launched!";
                }
                catch (Exception ex)
                {
                    launcherText.Text = $"Error: {ex.Message}";
                }
                finally
                {
                    playButton.IsEnabled = true;
                    Application.Current.Shutdown();
                }
            }
            startGame();
        }
    }
}