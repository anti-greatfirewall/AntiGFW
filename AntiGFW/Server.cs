using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

// ReSharper disable InconsistentNaming

namespace AntiGFW {
    [Serializable]
    public class Server {
        public static readonly Regex
            UrlFinder = new Regex(@"ss://(?<base64>[A-Za-z0-9+-/=_]+)(?:#(?<tag>\S+))?", RegexOptions.IgnoreCase),
            DetailsParser = new Regex(@"^((?<method>.+?):(?<password>.*)@(?<hostname>.+?):(?<port>\d+?))$", RegexOptions.IgnoreCase);

        private const int DefaultServerTimeoutSec = 5;
        public const int MaxServerTimeoutSec = 20;
        
        public string server;
        public int server_port;
        public string password;
        public string method;
        public string plugin;
        public string plugin_opts;
        public string remarks;
        public int timeout;

        public Server() {
            server = "";
            server_port = 8388;
            method = "aes-256-cfb";
            plugin = "";
            plugin_opts = "";
            password = "";
            remarks = "";
            timeout = DefaultServerTimeoutSec;
        }

        public static List<Server> GetServers(string ssUrl) {
            MatchCollection matches = UrlFinder.Matches(ssUrl);
            if (matches.Count <= 0) return null;
            List<Server> servers = new List<Server>();
            foreach (Match match in matches) {
                Server tmp = new Server();
                string base64 = match.Groups["base64"].Value;
                string tag = match.Groups["tag"].Value;
                if (!string.IsNullOrEmpty(tag)) {
                    tmp.remarks = HttpUtility.UrlDecode(tag, Encoding.UTF8);
                }
                Match details = DetailsParser.Match(Encoding.UTF8.GetString(Convert.FromBase64String(
                    base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '='))));
                if (!details.Success)
                    continue;
                tmp.method = details.Groups["method"].Value;
                tmp.password = details.Groups["password"].Value;
                tmp.server = details.Groups["hostname"].Value;
                tmp.server_port = int.Parse(details.Groups["port"].Value);

                servers.Add(tmp);
            }
            return servers;
        }
    }
}
