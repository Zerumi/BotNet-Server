// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Diagnostics;
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
                    Stopwatch stopwatch = new Stopwatch();
                    await LogPanel.Dispatcher.BeginInvoke(new Action(async() =>
                    {
                        stopwatch.Start();
                        LogPanel.Text = m3md2.StaticVariables.Diagnostics.ProgramInfo + "// Обнавляется каждую секунду.";
                        stopwatch.Stop(); 
                        if (LogPanel.Text.Length >= 100000 && !m3md2.StaticVariables.Settings.IgnoreBigLog)
                        {
                            CanListen = false;
                            await LogPanel.Dispatcher.BeginInvoke(new Action(() => LogPanel.Text.Replace("// Обнавляется каждую секунду.", "")));
                            MessageBox.Show("Обновление большого количества информации может нарушить высокую производительность системы. Мы приостановили вечное обновление информации, но вы можете всегда выгрузить информацию аудита в текстовый файл", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                            await StopListen.Dispatcher.BeginInvoke(new Action(() => {
                                StopListen.Content = "Обновление невозможно";
                                StopListen.IsEnabled = false;
                            }));
                        }

                    }));
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
            File.WriteAllText(filepatch, m3md2.StaticVariables.Diagnostics.ProgramInfo);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CanListen = false;
        }
    }
}