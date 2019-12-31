// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для Diagnostic2.xaml
    /// </summary>
    public partial class Diagnostic2 : Window
    {
        bool CanListen = true;

        public Diagnostic2()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(new Action(async() =>
            {
                while (true)
                {
                    if (!CanListen)
                    {
                        return;
                    }
                    await LogPanel.Dispatcher.BeginInvoke(new Action(() => LogPanel.Text = m3md2.StaticVariables.Diagnostics.ProgramInfo + "// Обнавляется каждую секунду."));
                    Thread.Sleep(1000);
                }
            }));
        }

        private void StopListen_Click(object sender, RoutedEventArgs e)
        {
            if (CanListen)
            {
                CanListen = false;
                StopListen.Content = "Запустить прослушку событий";
            }
            else
            {
                CanListen = true;
                Window_Loaded(new object(), new RoutedEventArgs());
                StopListen.Content = "Остановить прослушку событий";
            }
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            string dirpath = $@"{Directory.GetCurrentDirectory()}\Logs";
            Directory.CreateDirectory(dirpath);
            string filepatch = $@"{dirpath}\Logs{DateTime.Now.Ticks}.txt";
            File.Create(filepatch).Close();
            File.WriteAllText(filepatch, LogPanel.Text);
        }
    }
}