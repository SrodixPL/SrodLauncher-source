using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using CmlLib.Core.ProcessBuilder;
using CmlLib.Core;
using SrodLauncher_v2._0.MVVM.View;
using System.Windows.Controls;
using System.IO;
using System.Text.Json;
using System.Windows;
using CmlLib.Core.Installer.Forge;
using CmlLib.Core.Installer.Forge.Versions;
using CmlLib.Core.Version;
using CmlLib.Core.Installers;
using System.Net.Http;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;

namespace SrodLauncher_v2._0.Core
{
    internal class LaunchSP
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
        private Button playButton;
        private string packPath;

        public LaunchSP(SettingsView settingsView, SrodPackView srodPackView)
        {
            var settings = SettingsReader.ReadSettings();
            username = settings.Username;
            ram = settings.Ram;
            launcherText = srodPackView.GetLauncherText();
            playButton = srodPackView.GetPlayButton();
            playButton.IsEnabled = true;

            packPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                ".minecraft\\SrodPack");

            StartGame();
        }

        private async void StartGame()
        {
            try
            {
                playButton.IsEnabled = false;
                launcherText.Text = "Preparing to launch...";

                var path = new MinecraftPath(packPath);

                launcherText.Text = $"Using Minecraft path: {path.BasePath}";
                await Task.Delay(1000);

                await InstallSP();

                var launcher = new MinecraftLauncher(path);

                launcherText.Text = "Checking Minecraft installation...";
                await Task.Delay(500);

                await launcher.InstallAsync("1.20.1");
                launcherText.Text = "Minecraft 1.20.1 installed/verified";
                await Task.Delay(1000);

                launcherText.Text = "Setting up Forge installer...";
                var forge = new ForgeInstaller(launcher);

                launcherText.Text = "Installing Forge 47.4.0 for Minecraft 1.20.1...";
                var installTask = forge.Install("1.20.1", "47.4.0", new ForgeInstallOptions { });

                if (await Task.WhenAny(installTask, Task.Delay(TimeSpan.FromMinutes(5))) == installTask)
                {
                    version = await installTask;
                    launcherText.Text = "Forge installation completed!";
                }
                else
                {
                    throw new TimeoutException("Forge installation timed out after 5 minutes");
                }

                await Task.Delay(1000);

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
                    "Launching Minecraft with Forge (Premium account)..." :
                    "Launching Minecraft with Forge (Offline mode)...";

                var process = await launcher.CreateProcessAsync(version, launchOption);

                launcherText.Text = "Starting Minecraft process...";
                process.Start();
                launcherText.Text = "Minecraft launched successfully!";

                await Task.Delay(3000);
            }
            catch (Exception ex)
            {
                launcherText.Text = $"Error: {ex.Message}";
                MessageBox.Show($"Launch failed: {ex.Message}\n\nStack trace:\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                playButton.IsEnabled = true;
                return;
            }
            finally
            {
                playButton.IsEnabled = true;
                if (!launcherText.Text.StartsWith("Error:"))
                {
                    Application.Current.Shutdown();
                }
            }
        }

        private async Task InstallSP()
        {
            try
            {
                string packUrl = "https://github.com/SrodixPL/SrodPack/archive/refs/heads/main.zip";
                string tempZipPath = Path.Combine(Path.GetTempPath(), "srodpack.zip");
                string extractPath = Path.Combine(Path.GetTempPath(), "SrodPackExtract");

                string versionMarkerFile = Path.Combine(packPath, "pack_installed.marker");
                if (File.Exists(versionMarkerFile))
                {
                    launcherText.Text = "SrodPack already installed.";
                    return;
                }

                Directory.CreateDirectory(packPath);
                if (Directory.Exists(extractPath))
                    Directory.Delete(extractPath, true);
                Directory.CreateDirectory(extractPath);

                launcherText.Text = "Downloading SrodPack...";
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetAsync(packUrl);
                    using (var fs = new FileStream(tempZipPath, FileMode.Create))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }

                launcherText.Text = "Extracting SrodPack...";
                System.IO.Compression.ZipFile.ExtractToDirectory(tempZipPath, extractPath);

                string extractedDir = Directory.GetDirectories(extractPath).FirstOrDefault();
                if (string.IsNullOrEmpty(extractedDir))
                {
                    throw new DirectoryNotFoundException("Could not find extracted SrodPack directory");
                }

                launcherText.Text = "Installing SrodPack files...";
                CopyDirectory(extractedDir, packPath);

                File.WriteAllText(versionMarkerFile, DateTime.Now.ToString());

                try
                {
                    File.Delete(tempZipPath);
                    Directory.Delete(extractPath, true);
                }
                catch
                {
                    // ignore cleanup errors
                }

                launcherText.Text = "SrodPack installation complete.";
            }
            catch (Exception ex)
            {
                launcherText.Text = $"Pack installation error: {ex.Message}";
                throw;
            }
        }

        private void CopyDirectory(string sourceDir, string destDir)
        {
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (string dir in Directory.GetDirectories(sourceDir))
            {
                string destSubDir = Path.Combine(destDir, Path.GetFileName(dir));
                CopyDirectory(dir, destSubDir);
            }
        }
    }
}