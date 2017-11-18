using System.IO;

namespace AntiGFW {
    internal class Program {
        internal static void Main() {
            Utils.Initialize();
            bool update = File.Exists($"{Utils.ExeDirectory}settings.json");
            VpnConfigUpdater vpnUpdater = new VpnConfigUpdater();
            ConfigUpdater updater = new ConfigUpdater();

            if (update) {
                updater.Start();
                vpnUpdater.Start();
            } else {
                ShadowsocksDownloader downloader = new ShadowsocksDownloader();
                downloader.Start();
            }
        }
    }
}
