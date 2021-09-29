using System;
using SharpAdbClient;
using System.Net;
using System.IO;
using Ionic.Zip;

namespace ADBForwarder
{
    class Program
    {
        //const string sidequest = "\\Programs\\SideQuest\\resources\\app.asar.unpacked\\build\\platform-tools\\adb.exe";
        //static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static string adbPath = Path.GetTempPath() + "\\adb\\platform-tools\\adb.exe";
        static Uri uri = new Uri("https://dl.google.com/android/repository/platform-tools-latest-windows.zip");


        static void Main(string[] args)
        {
            var endPoint = new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort);

            AdbServer server = new AdbServer();

            if(!File.Exists(adbPath))
            {
                Console.WriteLine("ADB not found, downloading in the background...");
                DownloadADB();
                Console.WriteLine("Download Successful, starting ADB Daemon...");
                var result = server.StartServer(adbPath, restartServerIfNewer: false);
                
            }
            else
            {
                var result = server.StartServer(adbPath, restartServerIfNewer: false);
                Console.WriteLine("Starting ADB Daemon...");
            }

            var client = new AdbClient();
            client.Connect(endPoint);

            var devices = client.GetDevices();


            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                foreach (var device in devices)
                {
                    if (device.Name == "hollywood")
                    {
                        try
                        {
                            client.CreateForward(device, 9943, 9943);
                            client.CreateForward(device, 9944, 9944);
                        }
                        catch
                        {
                            //do nothing
                            //TODO: make this better
                        }
                    }
                }
            }
        }

        static void DownloadADB()
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(uri, "adb.zip");

                using (ZipFile zip = ZipFile.Read("adb.zip"))
                {
                    zip.ExtractAll(Path.GetTempPath() + "\\adb",
                    ExtractExistingFileAction.DoNotOverwrite);
                }
            }
        }
    }
 }

