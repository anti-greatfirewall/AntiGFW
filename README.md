# AntiGFW 1.2

[![License](https://img.shields.io/badge/license-GNU--GPLv3-blue.svg)](https://github.com/anti-greatfirewall/AntiGFW/blob/master/LICENSE)
[![GitHub issues](https://img.shields.io/github/issues/anti-greatfirewall/AntiGFW.svg?maxAge=2592000?style=flat-square)](https://github.com/anti-greatfirewall/AntiGFW/issues)

AntiGFW会根据您的配置文件从制定的网站抓取Shadowsocks的账号密码并自动更新，您可以参照以下教程填写配置文件。

## 文件结构

配置文件应放入一个文件夹，文件夹的结构如下:

 - 文件夹
   - settings.json
   - Shadowsocks
     - *各个版本的Shadowsocks文件夹以及文件*

## settings.json

此部分参照[暂缺]旧版本的配置文件和文件中的注释：

:warning:请使用前删除json中的注释!

若不清楚配置，请点击[这里](view-source:https://raw.githubusercontent.com/anti-greatfirewall/AntiGFW/master/Extract.c.txt)复制源代码并编译运行，即解压即用~

```json
{
    "websites": [ //用于将网站代码下载至指定文件
        {
            "url": "example.com", //网页地址
            "file": "file" //目标文件名(不可相同)
        }
    ],
    "plaintexts": [ //提供明文密码网站
        {
            "file": "file", //文件名
            "url": "ss.example.com", //服务器
            "port": 2333, //服务器端口
            "pwdprefix": "pwdpwd", //密码字符串在源代码中的前缀
            "length": 8, //密码长度
            "method": "aes-256-cfb", //加密方式
            "remarks": "Test" //服务器备注
        }
    ],
    "QRCodes": [
        { //二维码提供网站
            "url": "qrcode.example.com",
            "remarks": "Test"
        }
    ],
    "statics": [
        { //静态(密码不改变)的服务器
            "server": "ss.example.com", //服务器
            "server_port": 443, //服务器端口
            "password": "password", //密码
            "method": "aes-256-cfb", //加密方式
            "remarks": "Test", //服务器备注
            "timeout": 5
        }
    ],
    "versions": [ //Shadowsocks版本列表
        "4.0.6" //代表Shadowsocks\4.0.6\...
    ],
    "autorun": { //自动运行
        "versionIndex": 1 //版本编号，从1开始，参见上面"versions"中的顺序
    },
    "shadowsocksConfig": { //Shadowsocks配置
        "global": false, //是否开启全局模式
        "enabled": true, //是否启动代理
        "shareOverLan": false, //是否局域网共享
        "autoCheckUpdate": true, //是否自动检查更新
        "index": -1 //默认选择的服务器索引，-1为高可用，-2为负载均衡，-3为根据统计
    },
    "pacUrl": { //PAC设置
        "enabled": false, //是否启动在线PAC
        "staticUrl": true, //是否使用静态URL
        "pacUrl": "https://pac.itzmx.com/abc.pac", //静态PAC URL
        "dynamicPac": { //动态URL(有BUG请勿使用)
            "url": "https://freepac.co",
            "pacprefix": "\"text\">:",
            "length": 28
        }
    },
    "shadowsocksPath": null, //Shadowsocks的上层目录的路径，如C:\Shadowsocks则填"C:\\"
    "autorunEnabled": true, //是否自动运行
    "hourlyStartup": false, //是否一小时启动一次
    "autoStartup": false //是否开机启动
}
```

## Shadowsocks目录

在程序目录下没有settings.json时，程序将自动下载最新版Shadowsocks并配置好settings.json。

或者点击[这里](https://github.com/shadowsocks/shadowsocks-windows/releases)下载Shadowsocks，将Shadowsocks.exe放入目录。

## 程序使用

若autorunEnabled = true，程序将自动将配置目录地址设置为程序所在的目录，密码获取完毕后，按照提示即可。
否则程序将自动读取auto.txt中的内容自动运行。
