using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MoritzBernhoferDoor11
{
    /// <summary>
    /// Interaction logic for Snowflakes.xaml
    /// </summary>
    public partial class Snowflakes : Window
    {
        private List<Image> image = new();

        private DispatcherTimer generateSnowTimer = new();
        private DispatcherTimer fallingtimer = new();
        private DispatcherTimer heighchecker = new();

        private Random rdr = new();


        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExTransparent(hwnd);
        }
        public Snowflakes()
        {
            InitializeComponent();

            generateSnowTimer.Tick += GenerateSnowflakes;
            generateSnowTimer.Interval = TimeSpan.FromSeconds(3);

            fallingtimer.Tick += UpdateSnowflakes;
            fallingtimer.Interval = TimeSpan.FromMilliseconds(100);

            heighchecker.Tick += CheckSnowFlakeHeight;
            heighchecker.Interval = TimeSpan.FromMilliseconds(500);

            generateSnowTimer.Start();
            fallingtimer.Start();
            heighchecker.Start();
        }
        private void GenerateSnowflakes(object? sender, EventArgs args)
        {
            for (int i = 0; i < 9; i++)
            {
                int posX = rdr.Next(0, 1920);
                int posY = rdr.Next(0, 30);
                int image = rdr.Next(1, 4);
                int imageScale = rdr.Next(25, 35);

                Image img = new();
                img.Width = imageScale;

                var path = System.IO.Path.GetFullPath($"asset{image}.png");
                img.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
                img.Opacity = 0.7;

                Canvas.SetTop(img, posY);
                Canvas.SetLeft(img, posX);
                MyCanvas.Children.Add(img);
            }
        }
        private void UpdateSnowflakes(object? sender, EventArgs args)
        {
            foreach (Image child in MyCanvas.Children)
            {
                double height = Canvas.GetTop(child);
                Canvas.SetTop(child, height + 1);
            }
        }
        private void CheckSnowFlakeHeight(object? sender, EventArgs args)
        {
            foreach (Image child in MyCanvas.Children)
            {
                double height = Canvas.GetTop(child);
                if (height >= 470)
                {
                    MyCanvas.Children.Remove(child);
                }
            }
        }
    }
    public static class WindowsServices
    {
        const int WS_EX_TRANSPARENT = 0x00000020;
        const int GWL_EXSTYLE = (-20);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }
    }
}
