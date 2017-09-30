using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace AntiGFW {
    internal static class Extensions {
        public static bool IsNullOrEmpty(this string value) {
            return string.IsNullOrEmpty(value);
        }
    }

    internal class Utils {
        private class Dummy { }
        
        public static string ExePath => typeof(Dummy).Assembly.Location;
        public static string ExeDirectory => AppDomain.CurrentDomain.BaseDirectory;

        public static RegistryKey OpenRegKey(string name, bool writable, RegistryHive hive = RegistryHive.CurrentUser) {
            RegistryKey result;
            try {
                RegistryKey registryKey = RegistryKey.OpenBaseKey(hive, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(name, writable);
                result = registryKey;
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                result = null;
            }
            return result;
        }
    }

    internal static class AutoStartup {
        private static readonly string ExecutablePath = Utils.ExePath;
        private static readonly string Key = "AntiGFW_" + Utils.ExeDirectory.GetHashCode();

        public static bool Enabled {
            set => Set(value);
        }

        private static void Set(bool enabled) {
            try {
                RegistryKey registryKey = Utils.OpenRegKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (enabled) {
                    registryKey.SetValue(Key, ExecutablePath);
                } else {
                    registryKey.DeleteValue(Key);
                }
            } catch {
                // ignored
            }
        }
    }
}
