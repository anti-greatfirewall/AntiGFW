using System;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;

namespace AntiGFW {
    internal class ShadowsocksDownloader {
        public void Start() {
            string path = Utils.ExeDirectory;
            Console.WriteLine("Download Repository Page");
            string html = Utils.DownloadString("https://api.github.com/repos/shadowsocks/shadowsocks-windows/releases/latest");

            Console.WriteLine("\nExtract Download Link");
            string ver = "";
            for (int i = html.IndexOf("\"name\": \"", StringComparison.Ordinal) + 9; html[i] != '"'; i++) {
                ver += html[i];
            }

            Console.WriteLine($"\nDownload Shadowsocks {ver}");
            Directory.CreateDirectory($@"{path}\Shadowsocks");
            Directory.CreateDirectory($@"{path}\Shadowsocks\{ver}");
            string zip = $@"{path}\Shadowsocks\{ver}\{ver}.zip";
            Utils.DownloadDataProgress($@"https://github.com/shadowsocks/shadowsocks-windows/releases/download/{ver}/Shadowsocks-{ver}.zip", zip);

            Console.WriteLine($"\nUnzip Shadowsocks {ver} & Prepare Config");
            ZipFile.ExtractToDirectory(zip, $@"{path}\Shadowsocks\{ver}");
            Config config = new Config();
            config.versions.Add(ver);

            Console.WriteLine("\nWrite Config");
            File.WriteAllText("settings.json", JsonConvert.SerializeObject(config, Formatting.Indented));

            Console.WriteLine("\nDone.");
            Console.Read();
        }
    }
}
