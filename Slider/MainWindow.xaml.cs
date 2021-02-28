using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Slider
{
    public partial class MainWindow : Window
    {
        private TextBlock QrCodeGuide;
        private DoubleAnimation FadeInAnimation;
        private DoubleAnimation FadeOutAnimation;
        private static Image StaticQRCodeImage;
        private DoubleAnimation HighLightAnimation;
        private DoubleAnimation LowLightAnimation;

        public MainWindow()
        {
            InitializeComponent();
            App.Menu = DeviceMenu;
            App.ControlTemplate = (ControlTemplate)FindResource("DeviceMenuItemTemplate");
            App.DeviceCounter = Counter;
            StaticQRCodeImage = QRCodeImage;
            App.StartServer();
            UpdateQRCodeRequest();
            App.ListenerThread.Start();

            NotifyIcon ni = new NotifyIcon
            {
                Icon = new System.Drawing.Icon("icon.ico"),
                Visible = true
            };
            ni.DoubleClick += ShowWindow;
            System.Windows.Forms.MenuItem MenuItemShow = new System.Windows.Forms.MenuItem()
            {
                Text = "Show"
            };
            MenuItemShow.Click += ShowWindow;

            System.Windows.Forms.MenuItem MenuItemExit = new System.Windows.Forms.MenuItem()
            {
                Text = "Exit"
            };
            MenuItemExit.Click += Exit;


            ni.ContextMenu = new System.Windows.Forms.ContextMenu(new System.Windows.Forms.MenuItem[] {
                MenuItemShow, MenuItemExit
            });

            QrCodeGuide = qrCodeGuide;

            FadeInAnimation = new DoubleAnimation
            {
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(400))
            };

            FadeOutAnimation = new DoubleAnimation
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(400))
            };

            HighLightAnimation = new DoubleAnimation
            {
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(250))
            };

            LowLightAnimation = new DoubleAnimation
            {
                To = .5,
                Duration = new Duration(TimeSpan.FromMilliseconds(250))
            };
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Hide();
            base.OnStateChanged(e);
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown(0);
        }

        private void TitleBar_MouseLeftBtnDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ShowWindow(object sender, EventArgs args)
        {
            Show();
            WindowState = WindowState.Normal;
       }

        private void QRCodeMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            FadeInAnimation.From = QrCodeGuide.Opacity;
            QrCodeGuide.BeginAnimation(TextBlock.OpacityProperty, FadeInAnimation);
        }

        private void QRCodeMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            FadeOutAnimation.From = QrCodeGuide.Opacity;
            QrCodeGuide.BeginAnimation(TextBlock.OpacityProperty, FadeOutAnimation);

        }

        private void DeviceCounterClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            (FindResource("OpenDeviceMenu") as Storyboard).Begin();
        }

        private void ReturnButtonPressed(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            (FindResource("CloseDeviceMenu") as Storyboard).Begin();
        }

        public static void UpdateQRCodeRequest()
        {
            StaticQRCodeImage.Source = App.GenerateQRCode();
        }

        private void HighlightZoneEntered(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBlock target = (TextBlock)sender;
            target.BeginAnimation(OpacityProperty, HighLightAnimation);
        }

        private void HighlightZoneExited(object sender, System.Windows.Input.MouseEventArgs e)
        {
            TextBlock target = (TextBlock)sender;
            target.BeginAnimation(OpacityProperty, LowLightAnimation);

        }

        private void ToggleSinglePresenterMode(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            App.ToggleSinglePresenterMode();
            if (App.SinglePresenterMode)
            {
                SinglePresenterModeBtn.Text = "ON";
                SinglePresenterModeBtn.Foreground = new SolidColorBrush(Colors.Cyan);
                SinglePresenterModeDescription.BeginAnimation(OpacityProperty, FadeInAnimation);
            }
            else
            {
                SinglePresenterModeBtn.Text = "OFF";
                SinglePresenterModeBtn.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x7b));
                SinglePresenterModeDescription.BeginAnimation(OpacityProperty, FadeOutAnimation);
            }
        }
        
        private void Exit(object sender, EventArgs args)
        {
            System.Windows.Application.Current.Shutdown(0);
        }

        private void Minimize(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ExitButtonClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            (FindResource("ExitPrompt") as Storyboard).Begin();
        }

        private void ExitPromptDenied(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            (FindResource("ExitPromptReverse") as Storyboard).Begin();
        }
    }
}
