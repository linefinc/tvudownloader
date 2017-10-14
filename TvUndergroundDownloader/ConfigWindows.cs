using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TvUndergroundDownloaderLib;

namespace TvUndergroundDownloader
{
    [ComVisible(false)]
    public class ConfigWindows : Config
    {
        public ConfigWindows()
            : base()
        {
            //
            //  Enable web browser emulation
            //
            EnableWebBrowserEmulation();
        }


        public void Load()
        {
            if (File.Exists(FileNameConfig))
            {
                base.Load(FileNameConfig);
            }
        }

        public string ConfigFolder
        {
            get
            {
#if DEBUG
                return AppDomain.CurrentDomain.BaseDirectory;
#else

                // base path = C:\Users\User\AppData\Local\TvUndergroundDownloader\TvUndergroundDownloader\version
                // return  C:\Users\User\AppData\Local\TvUndergroundDownloader
                string basePath = Application.LocalUserAppDataPath;
                basePath = Directory.GetParent(basePath).FullName;
                basePath = Directory.GetParent(basePath).FullName;
                return basePath;
#endif
            }
        }

        public override string FileNameConfig
        {
            get
            {
                string fileName = "Config.xml";
                return Path.Combine(ConfigFolder, fileName);
            }
        }

        public override string FileNameLog
        {
            get
            {
                string fileName = "log.txt";
                return Path.Combine(ConfigFolder, fileName);
            }
        }

        /// <summary>
        /// Allow Browser Emulation
        /// </summary>
        private static void EnableWebBrowserEmulation()
        {
            RegistryKey hkcu = Registry.CurrentUser;
            hkcu = hkcu.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (hkcu != null)
            {
                hkcu.SetValue(Application.ProductName, 0x00002ee1, RegistryValueKind.DWord);
            }

            hkcu = Registry.CurrentUser;
            hkcu = hkcu.OpenSubKey(@"Software\WOW6432Node\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (hkcu != null)
            {
                hkcu.SetValue(Application.ProductName, 0x00002ee1, RegistryValueKind.DWord);
            }
        }

        public static bool StartWithWindows
        {
            get
            {
                RegistryKey hkcu = Registry.CurrentUser;
                hkcu = hkcu.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", RegistryKeyPermissionCheck.ReadWriteSubTree);

                List<string> RegValueNames = new List<string>();
                foreach (string valueName in hkcu.GetValueNames())
                {
                    RegValueNames.Add(valueName);
                }

                if (RegValueNames.IndexOf(Application.ProductName) == -1)
                {
                    return false;
                }
                return true;
            }
            set
            {
                RegistryKey hkcu = Registry.CurrentUser;
                hkcu = hkcu.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", RegistryKeyPermissionCheck.ReadWriteSubTree);

                if (value == true)
                {
                    hkcu.SetValue(Application.ProductName, Environment.CommandLine, RegistryValueKind.String);
                    return;
                }

                List<string> RegValueNames = new List<string>();
                foreach (string valueName in hkcu.GetValueNames())
                {
                    RegValueNames.Add(valueName);
                }

                if (RegValueNames.IndexOf(Application.ProductName) == -1)
                {
                    return;
                }
                hkcu.DeleteValue(Application.ProductName);
            }
        }
    }
}