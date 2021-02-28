using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Slider
{
    public partial class App : System.Windows.Application
    {
        public static ControlTemplate ControlTemplate;
        public static System.Windows.Controls.Menu Menu;
        public static TextBlock DeviceCounter;

        public delegate void CreateMenuItemDelegate(Connection instance, string header);
        public delegate void RemoveMenuItemDelegate(Connection instance);
        public delegate void UpdateMenuItemDelegate();
        public delegate void UpdateQRCodeRequestDelegate();
        private static Thread listenerThread = new Thread(new ThreadStart(StartListenerThread))
        {
            IsBackground = true
        };
        public static Thread ListenerThread
        {
            get
            {
                return listenerThread;
            }
        }
        private static TcpListener server;
        private static string port;
        public static bool SinglePresenterMode { get; private set; } = false;

        private static string key;

        private static int deviceCount = 0;
        public static int DeviceCount { get; }
        private static int connectionIndex = 0;

        public static List<Connection> activeConnections { get; } = new List<Connection>();


        public static BitmapImage GenerateQRCode()
        {
            Zen.Barcode.CodeQrBarcodeDraw renderer = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            string request = GenerateRequest();
            //Debug.WriteLine("Generated request: " + request);
            System.Drawing.Image image = renderer.Draw(request, 50);
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        private static string GenerateRequest()
        {
            StringBuilder builder = new StringBuilder(port + ";");
            {
                string temp = Encoding.ASCII.GetString(GenerateKey());
                builder.Append(temp + ";");
                key = temp;
            }

            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (UnicastIPAddressInformation ip in adapter.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            builder.Append(ip.Address.ToString() + ",");
                        }
                    }
                }
            }

            string finalString = builder.ToString();

            //Check
            bool valid = true;
            string[] segments = finalString.Split(';');
            foreach (string segment in segments)
                if (segment == "" || segment == null) valid = false;

            Debug.WriteLine("Generated request: " + finalString);

            if (valid) return finalString;
            else
            {
                RestartServer();
                return GenerateRequest();
            }
        }

        private static byte[] GenerateKey()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] byteArray = new byte[16];
            while (true)
            {
                bool BreakOut = true;
                rng.GetBytes(byteArray);
                foreach(byte b in byteArray)
                {
                    if(b == (byte)';')
                    {
                        BreakOut = false;
                        break;
                    }
                }

                if (BreakOut)
                {
                    break;
                }
            }
            return byteArray;
        }


        public static void StartListenerThread()
        {
                Debug.WriteLine("Starting listener thread...");
                while (true)
                {
                    Connection connection = new Connection(connectionIndex++);
                    connection.StartReceiving(server.AcceptSocket());
                    activeConnections.Add(connection);
                }
        }

        private static void RestartServer()
        {
            Debug.WriteLine("Restarting server...");
            StopServer();
            StartServer();
        }

        private static void StopServer()
        {
            Debug.WriteLine("Stopping server...");
            server.Stop();
            server = null;
            port = "";
        }

        public static void StartServer()
        {
            Debug.WriteLine("Starting server...");
            server = new TcpListener(IPAddress.Any, 0);
            server.Start();
            port = ((IPEndPoint)server.LocalEndpoint).Port.ToString();
        }


        public static void OnDownButton()
        {
            SendKeys.SendWait("{LEFT}");
        }

        public static void OnUpButton()
        {
            SendKeys.SendWait("{RIGHT}");
        }

        public static bool VerifyKey(String KeyToVerify)
        {
            return KeyToVerify.Equals(key);
        }


        public static void AddDevice()
        {
            deviceCount++;
            DeviceCounter.Text = deviceCount.ToString();
        }

        public static void RemoveDevice()
        {
            deviceCount--;
            DeviceCounter.Text = deviceCount.ToString();
        }

        public static void ToggleSinglePresenterMode()
        {
            SinglePresenterMode = !SinglePresenterMode;
            if (SinglePresenterMode && Connection.SinglePresenterConnection !=null)
                Connection.SinglePresenterConnection.Unlock();
        }
    }
}
