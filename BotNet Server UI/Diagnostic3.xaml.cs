// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Windows;
using System.Windows.Media;
using static BotNet_Server_UI.ApiRequest;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для Diagnostic1.xaml
    /// </summary>
    public partial class Diagnostic3 : Window
    {
        SolidColorBrush brush = new SolidColorBrush(m3md2.StaticVariables.Settings.colors[0]);
        SolidColorBrush brush2 = new SolidColorBrush(m3md2.StaticVariables.Settings.colors[2]);

        public Diagnostic3()
        {
            InitializeComponent();
            Grid.Background = brush;
            LogPanel.Foreground = brush2;
            LogPanel.Background = brush;
        }

        private async void FormLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Progress.Maximum = 90.0;
                LogPanel.Text += "ApiRequest...\n";
                Progress.Value += 10;
                string test = await GetProductAsync<string>("sandbox");
                Progress.Value += 10;
                if (test == "Who are you?")
                {
                    LogPanel.Text += "Who are you?\n";
                }
                else
                {
                    LogPanel.Text += "Апи не отвечает?...\n";
                }
                Progress.Value += 10;
                uint teest = await GetProductAsync<uint>("sandbox/1");
                Progress.Value += 10;
                LogPanel.Text += "Я " + teest + ". Сделай мне кофе\n";
                Progress.Value += 10;
                var teeest = await CreateProductAsync("Make a coffee", "sandbox");
                Progress.Value += 10;
                LogPanel.Text += "Похоже это чайник... Уходим отсюда...\n";
                Progress.Value += 10;
                var teeeest = await DeleteProductsAsync("sandbox");
                Progress.Value += 10;
                LogPanel.Text += "Итоговый код " + teeeest;
                Progress.Value += 10;
                MessageBox.Show("Диагностика API завершена");
                Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
                MessageBox.Show("Во время проверки API возникла ошибка.");
                Close();
            }
        }
    }
}
