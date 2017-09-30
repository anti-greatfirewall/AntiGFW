using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace AntiGFW {
    public class ConfigUpdater {
        private readonly WebClient _wc;

        public ConfigUpdater(WebClient web) {
            _wc = web;
        }

        private static void TaskKill(string name) {
            foreach (Process p in Process.GetProcesses()) {
                if (p.ProcessName.ToLower() != name) continue;
                try {
                    Console.WriteLine(p.Id + " -> " + p.ProcessName);
                    p.Kill();
                } catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        private void QrCode(Config config, ref Configuration result) {
            foreach (Config.QRCode i in config.qrCodes) {
                try {
                    byte[] file = _wc.DownloadData(i.url);
                    Bitmap bmp = new Bitmap(new MemoryStream(file));
                    QRCodeDecoder decoder = new QRCodeDecoder();
                    QRCodeBitmapImage image = new QRCodeBitmapImage(bmp);
                    List<Server> servers = Server.GetServers(decoder.decode(image));
                    foreach (Server j in servers) {
                        j.remarks = i.remarks;
                        Console.WriteLine($"[{i.remarks}, {j.server}] -> {j.password}");
                    }
                    result.configs.AddRange(servers);
                } catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        public void Start() {
            Dictionary<string, string> content = new Dictionary<string, string>();
            Configuration result = new Configuration();

            Console.WriteLine("Read:");
            string settings = File.ReadAllText($"{Utils.ExeDirectory}settings.json");
            Config config = JsonConvert.DeserializeObject<Config>(settings);
            AutoStartup.Enabled = config.autoStartup;
            string path = config.configPath;

            if (config.hourlyStartup) {
                Console.WriteLine("TaskScheduler Setting...");
                Thread.Sleep(1000);
                config.hourlyStartup = false;
                TaskScheduler.CreateTask();
                return;
            }

            Console.WriteLine("Directory:");
            Console.WriteLine(path);

            Console.WriteLine("\nDownload:");
            try {
                foreach (Config.Website i in config.websites) {
                    content[i.file] = _wc.DownloadString(i.url);
                    Console.WriteLine($"{i.url}");
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nExtract:");
            try {
                foreach (Config.PlainText i in config.plaintexts) {
                    Server tmp = new Server {
                        server = i.url,
                        server_port = i.port,
                        remarks = i.remarks,
                        method = i.method,
                        timeout = 5
                    };
                    int pos = content[i.file].IndexOf(i.pwdprefix, StringComparison.Ordinal);
                    tmp.password = content[i.file].Substring(pos + i.pwdprefix.Length, i.length);
                    Console.WriteLine($"[{tmp.remarks}, {tmp.server}] -> {tmp.password}");
                    result.configs.Add(tmp);
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nDecode QRCode:");
            QrCode(config, ref result);

            Console.WriteLine("\nKill Task...");
            TaskKill("shadowsocks");
            if (config.autorunEnabled) {
                Console.WriteLine("\nDelay...");
                Thread.Sleep(1000);
            }

            Console.WriteLine("\nVersions:");
            for (int i = 0; i < config.versions.Count; i++) {
                Console.WriteLine($"{i + 1} -> {config.versions[i]}");
            }

            int num;
            if (config.autorunEnabled) {
                num = config.autorun.versionIndex;
            } else {
                Console.WriteLine("Please select the version:");
                string input = Console.ReadLine();
                bool flag = int.TryParse(input, out num);
                while (!flag || num < 1 || num > config.versions.Count) {
                    input = Console.ReadLine();
                    flag = int.TryParse(input, out num);
                }
            }
            num--;

            Console.WriteLine("\nWrite");
            try {
                result.autoCheckUpdate = config.shadowsocksConfig.autoCheckUpdate;
                result.enabled = config.shadowsocksConfig.enabled;
                result.shareOverLan = config.shadowsocksConfig.shareOverLan;
                result.global = config.shadowsocksConfig.global;
                result.index = config.shadowsocksConfig.index;
                switch (result.index) {
                    case -1: result.strategy = "com.shadowsocks.strategy.ha"; break;
                    case -2: result.strategy = "com.shadowsocks.strategy.balancing"; break;
                    case -3: result.strategy = "com.shadowsocks.strategy.scbs"; break;
                    default: result.strategy = ""; break;
                }

                result.configs.AddRange(config.statics);
                File.WriteAllText(path + @"\ShadowSocks\" + config.versions[num] + @"\gui-config.json", JsonConvert.SerializeObject(result));
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nOpen Shadowsocks " + config.versions[num]);
            try {
                Process.Start(path + @"\ShadowSocks\" + config.versions[num] + @"\Shadowsocks.exe");
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Done.");
            if (!config.autorunEnabled) {
                Console.Read();
            }
        }
    }
}
