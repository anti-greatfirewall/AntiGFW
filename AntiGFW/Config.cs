using System;
using System.Collections.Generic;

namespace AntiGFW {
    [Serializable]
    public class Config {
        [Serializable]
        public class Website {
            public string url, file;
        }

        [Serializable]
        public class PlainText {
            public string file, url, pwdprefix, method, remarks;
            public int port, length;
        }

        [Serializable]
        public class QRCode {
            public string url, remarks;
        }

        [Serializable]
        public class Autorun {
            public string configPath;
            public int versionIndex;
            public Autorun() {
                configPath = Environment.CurrentDirectory;
                versionIndex = 0;
            }
        }
        
        [Serializable]
        public class ShadowsocksConfig {
            public bool global, enabled, shareOverLan, autoCheckUpdate;
            public int index;

            public ShadowsocksConfig() {
                index = 0;
                global = false;
                enabled = true;
                shareOverLan = false;
                autoCheckUpdate = true;
            }
        }

        public List<Website> websites;
        public List<PlainText> plaintexts;
        public List<QRCode> qrCodes;
        public List<Server> statics;
        public List<string> versions;
        public Autorun autorun;
        public ShadowsocksConfig shadowsocksConfig;

        public bool autorunEnabled;
        public string configPath;
        
        public Config() {
            websites = new List<Website>();
            plaintexts = new List<PlainText>();
            qrCodes = new List<QRCode>();
            statics = new List<Server>();
            versions = new List<string>();
            autorun = new Autorun();
            shadowsocksConfig = new ShadowsocksConfig();

            autorunEnabled = false;
            configPath = "";
        }
    }
}
