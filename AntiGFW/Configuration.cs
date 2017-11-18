using System;
using System.Collections.Generic;
using System.Drawing;

// ReSharper disable InconsistentNaming

namespace AntiGFW {
    [Serializable]
    public class LogViewerConfig {
        public bool topMost;
        public bool wrapText;
        public bool toolbarShown;

        public Font Font { get; set; } = new Font("Consolas", 8F);

        public Color BackgroundColor { get; set; } = Color.Black;

        public Color TextColor { get; set; } = Color.White;

        public LogViewerConfig() {
            topMost = false;
            wrapText = false;
            toolbarShown = false;
        }
    }

    [Serializable]
    public class HotkeyConfig {
        public string switchSystemProxy;
        public string switchSystemProxyMode;
        public string switchAllowLan;
        public string showLogs;
        public string serverMoveUp;
        public string serverMoveDown;

        public HotkeyConfig() {
            switchSystemProxy = "";
            switchSystemProxyMode = "";
            switchAllowLan = "";
            showLogs = "";
            serverMoveUp = "";
            serverMoveDown = "";
        }
    }

    [Serializable]
    public class ProxyConfig {
        public const int ProxySocks5 = 0;
        public const int ProxyHttp = 1;

        public const int MaxProxyTimeoutSec = 10;
        private const int DefaultProxyTimeoutSec = 3;

        public bool useProxy;
        public int proxyType;
        public string proxyServer;
        public int proxyPort;
        public int proxyTimeout;

        public ProxyConfig() {
            useProxy = false;
            proxyType = ProxySocks5;
            proxyServer = "";
            proxyPort = 0;
            proxyTimeout = DefaultProxyTimeoutSec;
        }
    }

    [Serializable]
    public class Configuration {
        public List<Server> configs = new List<Server>();
        
        public string strategy;
        public int index;
        public bool global;
        public bool enabled;
        public bool shareOverLan;
        public bool isDefault;
        public int localPort;
        public string pacUrl;
        public bool useOnlinePac;
        public bool secureLocalPac = true;
        public bool availabilityStatistics;
        public bool autoCheckUpdate;
        public bool checkPreRelease;
        public bool isVerboseLogging;
        
        public LogViewerConfig logViewer;
        public ProxyConfig proxy;
        public HotkeyConfig hotkey;
    }
}