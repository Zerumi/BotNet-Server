using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<TextBox> textBoxes = new List<TextBox>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Send_Command()
        {
            if (Command.Text == "" || IP1.Text == "")
            {
                MessageBox.Show("Постарайтесь ввести все значения правильно!");
                return;
            }
            try
            {
                Message message = new Message()
                {
                    id = await ApiRequest.GetProductAsync<uint>("/api/v1/messages"),
                    command = Command.Text,
                    ip = new string[Convert.ToInt32(IPCount.Content.ToString())]
                };
                for (int i = 0; i < message.ip.Length; i++)
                {
                    message.ip[i] = textBoxes[i].Text;
                }
                var res = await ApiRequest.CreateProductAsync(message, "messages");
                LogPanel.Text += $"Сообщение {message.command} (id: {message.id}) отправлено.\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Send_Command();
        }

        private void Command_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Send_Command();
            }
        }

        private async void Formloaded(object sender, RoutedEventArgs e)
        {
            _ = await ApiRequest.DeleteProductsAsync("api/v1/messages");
            textBoxes.Add(IP1);
            textBoxes.Add(IP2);
            textBoxes.Add(IP3);
            textBoxes.Add(IP4);
            textBoxes.Add(IP5);
            textBoxes.Add(IP6);
            textBoxes.Add(IP7);
            textBoxes.Add(IP8);
            textBoxes.Add(IP9);
            textBoxes.Add(IP10);
            textBoxes.Add(IP11);
            textBoxes.Add(IP12);
            textBoxes.Add(IP13);
            textBoxes.Add(IP14);
            textBoxes.Add(IP15);
            textBoxes.Add(IP16);
            textBoxes.Add(IP17);
            textBoxes.Add(IP18);
            textBoxes.Add(IP19);
            textBoxes.Add(IP20);
            textBoxes.Add(IP21);
            textBoxes.Add(IP22);
            textBoxes.Add(IP23);
            textBoxes.Add(IP24);
            textBoxes.Add(IP25);
            textBoxes.Add(IP26);
            textBoxes.Add(IP27);
            textBoxes.Add(IP28);
            UpdateTextBoxes();
            var arr = await ApiRequest.GetProductAsync<IP[]>("/api/v1/ip");
            string res = "";
            for (int i = 0; i < arr.Length; i++)
            {
                res += arr[i].ip + "\n";
            }
            ClientList.Text = res;
            var info = await ApiRequest.GetProductAsync<Info>("/api");
            InfoLabel.Content = "Подключено к серверу: " + info.uri + "; API версии " + info.version + ".\nПрослушка по порту " + info.port + "; среда разработки " + info.environment + ".\nВсего клиентов " + info.clients + ". Всего сообщений " + info.messages;
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(IPCount.Content.ToString()) == 1)
            {
                MessageBox.Show("Значение не может быть ниже 1 и выше 28");
                return;
            }
            IPCount.Content = Convert.ToInt32(IPCount.Content.ToString()) - 1;
            UpdateTextBoxes();
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(IPCount.Content.ToString()) == 28)
            {
                MessageBox.Show("Значение не может быть ниже 1 и выше 28");
                return;
            }
            IPCount.Content = Convert.ToInt32(IPCount.Content.ToString()) + 1;
            UpdateTextBoxes();
        }

        private void UpdateTextBoxes()
        {
            for (int i = 0; i < textBoxes.Count; i++)
            {
                if (i < Convert.ToInt32(IPCount.Content.ToString()))
                {
                    textBoxes[i].Visibility = Visibility.Visible;
                }
                else
                {
                    textBoxes[i].Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
