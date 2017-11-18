using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.Win32;

//using System.Threading;

namespace AntiGFW {
    internal static class Extensions {
        public static bool IsNullOrEmpty(this string value) {
            return string.IsNullOrEmpty(value);
        }
    }

    internal static class Utils {
        private class Dummy { }

        public static string ExePath => typeof(Dummy).Assembly.Location;
        public static string ExeDirectory => AppDomain.CurrentDomain.BaseDirectory;

        // Windows 10; Chrome dev
        internal const string UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3213.3 Safari/537.36";
        public static WebClient wc =
        new WebClient {
            Encoding = Encoding.UTF8,
            Credentials = CredentialCache.DefaultCredentials
        };

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

        public static void Initialize() {
            wc.Headers.Set("User-Agent", UserAgent);
        }

        public static string DownloadString(string address) {
            return wc.DownloadString(address);
        }

        public static byte[] DownloadData(string address) {
            return wc.DownloadData(address);
        }

        public static void DownloadDataProgress(string srcUrl, string destFile) {
            DateTime start = DateTime.Now;
            int top = Console.CursorTop;
            Console.WriteLine("Process:");
            try {
                HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(srcUrl);
                HttpWebResponse webResponse = (HttpWebResponse) webRequest.GetResponse();
                long totalBytes = webResponse.ContentLength;
                Stream webStream = webResponse.GetResponseStream();
                FileStream fileStream = new FileStream(destFile, FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] data = new byte[1024];
                // ReSharper disable once PossibleNullReferenceException
                int osize = webStream.Read(data, 0, data.Length);
                while (osize > 0) {
                    totalDownloadedByte = osize + totalDownloadedByte;
                    fileStream.Write(data, 0, osize);
                    osize = webStream.Read(data, 0, data.Length);
                    double percent = totalDownloadedByte / (double) totalBytes * 100;
                    DateTime point = DateTime.Now;
                    Console.SetCursorPosition(0, top);
                    Console.WriteLine($"{Math.Round(percent, 2)}% {totalDownloadedByte / (point - start).TotalSeconds / 1000}KB/s");
                    Console.WriteLine($"{totalDownloadedByte}B/{totalBytes}B");
                    for (int i = 0; i < percent / 10; i++) {
                        Console.Write("█");
                    }
                }
                fileStream.Close();
                webStream.Close();
            } catch (Exception e) {
                Console.WriteLine(e);
            }
            Console.WriteLine("\nFinished!");
        }
    }

    internal static class AutoStartup {
        public static readonly string ExecutablePath = Utils.ExePath;
        public static readonly string Key = "AntiGFW_" + Utils.ExeDirectory.GetHashCode();

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
