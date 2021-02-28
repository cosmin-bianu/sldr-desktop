using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Slider
{
    public class Connection
    {
        private Thread connection;
        //private Thread checkerThread;
        private Socket socket;
        private string DeviceName;
        private MenuItem CorrespondingMenuItem;
        public int ConnectionIndex { get; private set; }
        public static Connection SinglePresenterConnection { get; private set; }
        private bool locked = false;

        public Connection(int index)
        {
            ConnectionIndex = index;
        }
        

        public void StartReceiving(Socket socket)
        {
            this.socket = socket;

            connection = new Thread(new ThreadStart(ReceiveData)) {
                IsBackground = true
            };

            connection.Start();
        }

        private void ReceiveData()
        {
            try
            {
                
                //Pre-receiving setup.
                byte[] rcvLenBytes;
                byte[] rcvBytes;
                
                //Verify key.
                rcvLenBytes = new byte[4];
                socket.Receive(rcvLenBytes);
                int rcvLen = BitConverter.ToInt32(rcvLenBytes, 0);
                if (rcvLen == 16)
                {
                    rcvBytes = new byte[16];
                    socket.Receive(rcvBytes);
                    string key = System.Text.Encoding.ASCII.GetString(rcvBytes);
                    Debug.WriteLine("Received key: " + key);
                    if (!App.VerifyKey(key))
                    {
                        //Debug.WriteLine("Return 1");
                        socket.Close();
                        return;
                    }
                }
                else
                {
                    //Debug.WriteLine("Return 2");
                    socket.Close();
                    return;
                }
                

                //Get device model.
                rcvLenBytes = new byte[4];
                socket.Receive(rcvLenBytes);
                rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
                rcvBytes = new byte[rcvLen];
                socket.Receive(rcvBytes);
                DeviceName = System.Text.Encoding.ASCII.GetString(rcvBytes);
                //Debug.WriteLine("DeviceName: " + DeviceName);
                App.Menu.Dispatcher
                    .Invoke(new App.CreateMenuItemDelegate(CreateMenuItem), new object[] { this, "    " + (ConnectionIndex+1).ToString() + "     " + DeviceName });


                byte[] byteBuffer;
                int byteSize;
                //checkerThread.Start();
                while (true)
                {

                    byteBuffer = new byte[1];
                    try
                    {
                        byteSize = socket.Receive(byteBuffer);
                    } catch (SocketException)
                    {
                        RemoveCorrespondingMenuItem();
                        break;
                    }
                    //Debug.WriteLine("byteSize: " + byteSize);
                    //Debug.WriteLine("byteBuffer[0]: " + byteBuffer[0]);
                    if (byteSize == 0)
                    {
                        RemoveCorrespondingMenuItem();
                        return;
                    }
                    switch (byteBuffer[0])
                    {
                        case 16:
                            break;
                        case 32:
                            App.DeviceCounter.Dispatcher.Invoke(new App.UpdateMenuItemDelegate(Lock),new object[] { });
                            break;
                        case 64:
                            App.DeviceCounter.Dispatcher.Invoke(new App.UpdateMenuItemDelegate(Unlock), new object[] { });
                            break;
                        case 127:
                            if(!locked) App.OnDownButton();
                            break;
                        case 255:
                            if(!locked) App.OnUpButton();
                            break;
                    }
                }
            }
            catch (ThreadInterruptedException)
            {
                RemoveCorrespondingMenuItem();
            }

            RemoveCorrespondingMenuItem();

        }

        public void Unlock()
        {
            SinglePresenterConnection = this;
            CorrespondingMenuItem.Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0xff, 0xff));
            locked = false;
        }

        private void LockUnselectedDevices()
        {
            if (App.SinglePresenterMode)
                foreach (Connection conn in App.activeConnections)
                    if (!conn.Equals(this)) conn.Lock();
                    else conn.Unlock();
        }

        private void Lock()
        {
            locked = true;
            CorrespondingMenuItem.Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0x00, 0x7b));
        }

        private static void CreateMenuItem(Connection instance, string header)
        {
            MenuItem returnValue = new MenuItem
            {
                Header = header,
                BorderBrush = new SolidColorBrush(Color.FromArgb(0x00, 0xff, 0Xff, 0xff)),
                Background = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00)),
                FontSize = 20,
                FontFamily = new FontFamily("Pulp Display Medium"),
                Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0xff, 0xff)),
                Width = 309,
                Height = 63,
                HorizontalAlignment = HorizontalAlignment.Left,
                Template = App.ControlTemplate,
            };
            returnValue.Click += new RoutedEventHandler(instance.MenuItemClick);
            App.Menu.Items.Add(returnValue);
            instance.CorrespondingMenuItem = returnValue;
            App.AddDevice();
            App.DeviceCounter.Dispatcher.Invoke(new App.UpdateQRCodeRequestDelegate(MainWindow.UpdateQRCodeRequest), new object[] { });
        }

        private void MenuItemClick(object sender, EventArgs e)
        {
            SetCurrentSinglePresenterModeItem(this);
            LockUnselectedDevices();
        }

        private void SetCurrentSinglePresenterModeItem(Connection connection)
        {
            SinglePresenterConnection = connection;
        }

        private void RemoveCorrespondingMenuItem()
        {
            App.Menu.Dispatcher.Invoke(new App.RemoveMenuItemDelegate(RemoveMenuItem), new object[] { this });
        }

        private static void RemoveMenuItem(Connection instance)
        {
            App.Menu.Items.Remove(instance.CorrespondingMenuItem);
            App.RemoveDevice();
        }
    }
}
