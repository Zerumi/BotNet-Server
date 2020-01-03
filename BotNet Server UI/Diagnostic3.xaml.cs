// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Windows;
using static BotNet_Server_UI.ApiRequest;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для Diagnostic1.xaml
    /// </summary>
    public partial class Diagnostic3 : Window
    {
        public Diagnostic3()
        {
            InitializeComponent();
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
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
                MessageBox.Show("Во время проверки API возникла ошибка.");
                Close();
            }
        }
    }
}
