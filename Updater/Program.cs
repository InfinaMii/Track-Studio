using System;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Updater
{
    internal class Program
    {
        static string execDirectory = "";

        static void Main(string[] args)
        {
            execDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            bool force = args.Contains("-f");
            args = ["-d", "-i", "-b"];
            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "-d":
                    case "--download":
                        UpdaterHelper.SetupOctokit("MapStudioProject", "Track-Studio");
                        UpdaterHelper.DownloadLatest(execDirectory, 0, force);
                        break;
                    case "-i":
                    case "--install":
                        UpdaterHelper.Install(execDirectory, "TrackStudio" + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".exe" : ""));
                        break;
                    case "-b":
                    case "--boot":
                        Boot();
                        Environment.Exit(0);
                        break;
                    case "-bl":
                    case "--boot_launcher":
                        BootLauncher();
                        Environment.Exit(0);
                        break;
                    case "-e":
                    case "--exit":
                        Environment.Exit(0);
                        break;
                }
            }
        }

        static void BootLauncher()
        {
            Console.WriteLine("Booting...");

            Thread.Sleep(3000);
            Process.Start(Path.Combine(execDirectory, "TrackStudioLauncher.exe"));
        }

        static void Boot()
        {
            Console.WriteLine("Booting...");

            Thread.Sleep(3000);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Process.Start(Path.Combine(execDirectory, "TrackStudio.exe"));
            else 
                Process.Start("sh", $"-c \"dotnet \'{Path.Combine(execDirectory, "TrackStudio.dll")}\'\"");
        }

    }
}
