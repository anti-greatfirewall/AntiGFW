using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace AntiGFW {
    internal class ShadowsocksDownloader {
        private readonly WebClient _wc;

        public ShadowsocksDownloader(WebClient web) {
            _wc = web;
        }

        public void Start(string path) {
            Config config = new Config
            {
                configPath = Environment.CurrentDirectory
            };

            Console.WriteLine("\nDownload Repository Page");
            string html = _wc.DownloadString("https://api.github.com/repos/shadowsocks/shadowsocks-windows/releases/latest");

            Console.WriteLine("\nExtract Download Link");
            string url = "", ver = "";
            for (int i = html.IndexOf("\"name\": \"", StringComparison.Ordinal) + 9; html[i] != '"'; i++) {
                ver += html[i];
            }
            for (int i = html.IndexOf("\"browser_download_url\": \"", StringComparison.Ordinal) + 25; html[i] != '"'; i++) {
                url += html[i];
            }

            Console.WriteLine("\nDownload Shadowsocks " + ver);
            Directory.CreateDirectory(path + @"\Shadowsocks");
            Directory.CreateDirectory(path + @"\Shadowsocks\" + ver);
            Console.WriteLine("Process:");
            DateTime start = DateTime.Now;
            bool exit = true;
            bool con = true;
            _wc.DownloadProgressChanged += (s, e) => {
                DateTime point = DateTime.Now;
                if (!con) {
                    return;
                }
                con = false;
                Console.Clear();
                Console.WriteLine($"{e.BytesReceived / (point - start).TotalSeconds / 1000}KB/s | {e.BytesReceived}B/{e.TotalBytesToReceive}B");
                for (int i = 0; i < e.ProgressPercentage / 10; i++) {
                    Console.Write("█");
                }
                con = true;
            };
            _wc.DownloadFileCompleted += (s, e) => {
                exit = false;
            };
            while (exit) { }
            Console.WriteLine("Finished!");
            string zip = path + @"\Shadowsocks\" + ver + @"\Shadowsocks-" + ver + ".zip";
            _wc.DownloadFileAsync(new Uri(url), zip);
            Console.Clear();

            Console.WriteLine("\nUnzip Shadowsocks " + ver + " & Prepare Config");
            ZipFile.ExtractToDirectory(zip, path + @"\Shadowsocks\" + ver);
            config.versions.Add(ver);

            Console.WriteLine("\nWrite Config");
            File.WriteAllText("settings.json", JsonConvert.SerializeObject(config));

            Console.WriteLine("\nDone.");
            Console.Read();
        }
    }
}
