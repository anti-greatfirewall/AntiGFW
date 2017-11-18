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
            public int versionIndex;
            public Autorun() {
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

        [Serializable]
        public class PacUrl {
            [Serializable]
            public class DynamicPac {
                public string url, pacprefix;
                public int length;

                public DynamicPac() {
                    url = pacprefix = null;
                    length = 0;
                }
            }

            public bool enabled;
            public bool staticUrl;
            public string pacUrl;
            public DynamicPac dynamicPac;

            public PacUrl() {
                enabled = false;
            }

            public string GetUrl() {
                if (!enabled) {
                    return null;
                }
                if (staticUrl) {
                    return pacUrl;
                }
                string html = Utils.DownloadString(dynamicPac.url);
                int pos = html.IndexOf(dynamicPac.pacprefix, StringComparison.Ordinal);
                return html.Substring(pos + dynamicPac.length, dynamicPac.length);
                //"text">
            }
        }

        public List<Website> websites;
        public List<PlainText> plaintexts;
        public List<QRCode> qrCodes;
        public List<Server> statics;
        public List<string> versions;
        public Autorun autorun;
        public ShadowsocksConfig shadowsocksConfig;
        public PacUrl pacUrl;

        public string shadowsocksPath;
        public bool autorunEnabled;
        public bool hourlyStartup;
        public bool autoStartup;
        
        public Config() {
            websites = new List<Website>();
            plaintexts = new List<PlainText>();
            qrCodes = new List<QRCode>();
            statics = new List<Server>();
            versions = new List<string>();
            autorun = new Autorun();
            shadowsocksConfig = new ShadowsocksConfig();

            shadowsocksPath = null;
            autorunEnabled = false;
            autoStartup = false;
            pacUrl = new PacUrl();
        }
    }
}
