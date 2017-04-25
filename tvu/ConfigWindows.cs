using Microsoft.Win32;
using System;
using System.Collections.Generic;
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