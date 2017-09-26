using System;
using System.IO;
using System.Net;
using System.Text;

namespace AntiGFW {
    internal class Program {
        internal const string UserAgent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3198.0 Safari/537.36";

        internal static void Main() {
            WebClient wc = new WebClient {
                Encoding = Encoding.UTF8,
                Credentials = CredentialCache.DefaultCredentials
            };
            wc.Headers.Set("User-Agent", UserAgent);
            
            ConfigUpdater updater = new ConfigUpdater(wc);
            bool update = File.Exists("settings.json");

            if (update) {
                updater.Start();
            } else {
                ShadowsocksDownloader downloader = new ShadowsocksDownloader(wc);
                downloader.Start(Environment.CurrentDirectory);
            }
        }
    }
}
