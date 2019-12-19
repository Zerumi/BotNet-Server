// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для ScreenBox.xaml
    /// </summary>
    public partial class ScreenBox : Window
    {
        readonly string nameofpc;
        public int screenid = 0;
        ScreenByte[] ScreenBytes { get; set; }
        public ScreenBox(ScreenByte[] screenBytes, string nameofpc)
        {
            this.nameofpc = nameofpc;
            this.ScreenBytes = screenBytes;
            InitializeComponent();
            var image = LoadImage(screenBytes[screenid].bytes);
            Image.Source = image;
        }
        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        private void Plus_Button_Click(object sender, RoutedEventArgs e)
        {
            if (++screenid == ScreenBytes.Length)
            {
                screenid--;
                return;
            }
            Image.Source = LoadImage(ScreenBytes[screenid].bytes);
        }

        private void Minus_Button_Click(object sender, RoutedEventArgs e)
        {
            if (screenid-- == 0)
            {
                screenid++;
                return;
            }
            Image.Source = LoadImage(ScreenBytes[screenid].bytes);
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
        linkcreatepng:
            try
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)Image.Source));
                using FileStream stream = new FileStream($"{Directory.GetCurrentDirectory()}\\Screens\\{new Random().Next()}-Screen-{nameofpc}-{ScreenBytes[screenid].sid}.png", FileMode.Create);
                encoder.Save(stream);
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\Screens");
                goto linkcreatepng;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
