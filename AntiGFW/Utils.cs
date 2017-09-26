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
        private static readonly string ExecutablePath = Process.GetCurrentProcess().MainModule.FileName;
        private static readonly string Key = "AntiGFW_" + Environment.CurrentDirectory.GetHashCode();

        public static void Set(bool enabled) {
            try {
                RegistryKey registryKey = Utils.OpenRegKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (enabled) {
                    registryKey.SetValue(Key, ExecutablePath);
                } else {
                    registryKey.DeleteValue(Key);
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
