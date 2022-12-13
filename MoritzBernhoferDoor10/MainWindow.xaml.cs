using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Brushes = System.Windows.Media.Brushes;

namespace MoritzBernhoferDoor11
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void UpdateSoundMeter(object? sender, EventArgs args)
        {
            MySoundMeter.Content = MyAudio.Adio;
        }
        private DispatcherTimer adioTimer = new();
        private class Cordinate
        {
            public Cordinate(int x, int y)
            {
                X = x;
                Y = y;
            }
            public int X { get; set; }
            public int Y { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int mouse_LEFTDOWN = 0x02;
        private const int mouse_LEFTUP = 0x04;
        private string? path;
        private Random random = new();
        private List<Cordinate> cordinates = new();
        private DispatcherTimer printtimer = new();
        private int idx = 0;
        private void MyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                if (File.Exists(MyTextBox.Text))
                {
                    MySucces.Foreground = Brushes.LightGreen;
                    MySucces.Content = "Succes";
                    path = MyTextBox.Text;
                    Thread.Sleep(400);
                    LoadPicture();
                    SelectPrintingType();
                }
                else
                {
                    MySucces.Visibility = Visibility.Visible;
                    MySucces.Content = "Error, File Not found";
                    MySucces.Foreground = Brushes.Red;
                }
            }
        }

        private void SelectPrintingType()
        {
            printtimer.Tick += PrintToPaintRowByRow;
            printtimer.Interval = TimeSpan.FromMilliseconds(10);
            printtimer.Start();
        }

        private void PrintToPaintRowByRow(object? sender, EventArgs e)
        {
            SetCursorPos(cordinates[idx].X, cordinates[idx].Y);
            idx += 10;
            mouse_event(mouse_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(mouse_LEFTUP, 0, 0, 0, 0);
            if (idx + 10 > cordinates.Count)
            {
                printtimer.Stop();
            }
        }


        private void LoadPicture()
        {
            Bitmap img = new Bitmap(path);
            int width = img.Width;
            int height = img.Height;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    System.Drawing.Color pixel = img.GetPixel(i, j);
                    double Brightness = (0.2126 * pixel.R + 0.7152 * pixel.G + 0.0722 * pixel.B);
                    // 37, 150, 190
                    if (Brightness < 190)
                    {
                        cordinates.Add(new Cordinate(i + 200, j + 300));
                    }
                }
            }
        }

        private void SnowButton(object sender, RoutedEventArgs e)
        {
            Snowflakes snowflakes = new();
            snowflakes.Show();
        }

        private void PlayMusic(object sender, RoutedEventArgs e)
        {
            SoundPlayer sp = new();
            MyAudio my = new();
            adioTimer.Tick += UpdateSoundMeter;
            adioTimer.Interval = TimeSpan.FromMilliseconds(500);
            adioTimer.Start();
            string path = System.IO.Path.GetFullPath("audio.wav");
            string[] lines = path.Split("MoritzBernhoferDoor10");
            path = lines[0] + @"MoritzBernhoferDoor10\Assets\audio.wav";
            sp.SoundLocation = path;
            sp.Play();
        }
    }
}
