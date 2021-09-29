using System;
using SharpAdbClient;
using System.Net;
using System.IO;
using System.Collections.Generic;
using Ionic.Zip;
using Usb.Events;

namespace ADBForwarder
{
    class Program
    {
        static string adbPath = Path.GetTempPath() + "\\adb\\platform-tools\\adb.exe";
        static AdbClient client = new AdbClient();
        static AdbServer server = new AdbServer();
        static Uri uri = new Uri("https://dl.google.com/android/repository/platform-tools-latest-windows.zip");
        static IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort);
        List<DeviceData> devices = new List<DeviceData>();
        

        static void Main(string[] args)
        {

            if(!File.Exists(adbPath))
            {
                Console.WriteLine("ADB not found, downloading in the background...");
                DownloadADB();
                Console.WriteLine($"Download Successful!\nLocated at: {adbPath}\nStarting ADB Daemon...");
                var result = server.StartServer(adbPath, restartServerIfNewer: false);
            }
            else
            {
                var result = server.StartServer(adbPath, restartServerIfNewer: false);
                Console.WriteLine("Starting ADB Daemon...");
            }

            client.Connect(endPoint);
            
            DeviceMonitor monitor = new DeviceMonitor(new AdbSocket(endPoint));
            monitor.DeviceConnected += Monitor_DeviceConnected;
            monitor.DeviceDisconnected += Monitor_DeviceDisconnected;
            
            monitor.Start();

            List<DeviceData> devices = client.GetDevices();

            while (true)
            {
                // Main thread needs to stay alive, 100ms is acceptable idle time
                System.Threading.Thread.Sleep(100);
            }
        }

        private static void Monitor_DeviceDisconnected(object sender, DeviceDataEventArgs e)
        {
            Console.WriteLine($"Event: Disconnection\n Device: {e.Device.Serial}");
        }

        private static void Monitor_DeviceConnected(object sender, DeviceDataEventArgs e)
        {

            Console.WriteLine($"Event: Connection\n Device: {e.Device.Serial}");
            System.Threading.Thread.Sleep(1000); // dont do shit immediately
            Forward();
        }

        static void Forward()
        {
            List<DeviceData> devices = client.GetDevices();

            foreach (var device in devices)
            {
                if(device.Name == "hollywood")
                {
                    client.CreateForward(device, 9943, 9943);
                    client.CreateForward(device, 9944, 9944);
                    Console.WriteLine("Forwarded!");

                }
            }
        }
        static void DownloadADB()
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(uri, Path.GetTempPath() + "\\adb.zip");

                using (ZipFile zip = ZipFile.Read("adb.zip"))
                {
                    zip.ExtractAll(Path.GetTempPath() + "\\adb",
                    ExtractExistingFileAction.DoNotOverwrite);
                }
                File.Delete(Path.GetTempPath() + "\\adb.zip");
            }
        }
    }
 }

