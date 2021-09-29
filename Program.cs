using System;
using SharpAdbClient;
using System.Net;

namespace ADBForwarder
{
    class Program
    {
        const string sidequest = "\\Programs\\SideQuest\\resources\\app.asar.unpacked\\build\\platform-tools\\adb.exe";
        static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static string adbPath = localAppData + sidequest; //i fucking hate this but path.combine was not having it

        static void Main(string[] args)
        {
            Console.WriteLine("starting");

            var endPoint = new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort);

            AdbServer server = new AdbServer();
            try
            {
                var result = server.StartServer(adbPath, restartServerIfNewer: false);
                Console.WriteLine("ADB Daemon Started");
            }
            catch
            {
                Console.WriteLine("SideQuest not installed.");
                Console.ReadKey();
                return;
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
    }
 }

