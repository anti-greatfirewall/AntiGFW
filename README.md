# AntiGFW 1.0

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

```
{
    "websites": [
        { //用于将网站代码下载至指定文件
            "url": "example.com",
            "file": "file"
        }
    ],
    "plaintexts": [
        { //提供明文密码网站
            "file": "file",
            "url": "ss.example.com",
            "port": 2333,
            "pwdprefix": "pwdpwd",
            "length": 8,
            "method": "aes-256-cfb",
            "remarks": "Just For Test!"
            "isSSUrl": false //留空默认为false
        },
        { //提供ss://链接网站
            "file": "file",
            "url": "ss.example.com",
            "port": 2333,
            "pwdprefix": "ssurl",
            "length": 8,
            "method": "aes-256-cfb",
            "remarks": "Just For ss:// Test!"
            "isSSUrl": true
        }
    ],
    "QRCodes": [
        { //二维码提供网站
            "url": "qrcode.example.com",
            "remarks": "Just For QRCode Test!"
        }
    ],
    "statics": [
        { //静态(密码不改变)的服务器
            "server": "ss.example.com",
            "server_port": 443,
            "password": "password",
            "method": "aes-256-cfb",
            "remarks": "Just For Static Text!",
            "timeout": 5
        }
    ],
    "versions": [
        "4.0.6"
    ],
    "autorun": { //自动运行
        "configPath": "C:\\path\\to\\config", //配置文件夹路径
        "versionIndex": 1 //版本编号
    },
    "autorunEnabled": false
}
```

## Shadowsocks目录

在程序目录下没有settings.json时，程序将自动下载最新版Shadowsocks并配置好settings.json。

或者点击[这里](https://github.com/shadowsocks/shadowsocks-windows/releases)下载Shadowsocks，将Shadowsocks.exe放入目录。

## 程序使用

若autorunEnabled = true，程序将自动将配置目录地址设置为程序所在的目录，密码获取完毕后，按照提示即可。
否则程序将自动读取auto.txt中的内容自动运行。
