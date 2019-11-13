using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
#pragma warning disable CS0618 // Тип или член устарел
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
#pragma warning restore CS0618 // Тип или член устарел
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint ipPoint = new IPEndPoint(ipAddress, 8005);

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    socket.Bind(ipPoint);

                    socket.Listen(10);
                    ServerLabel.Content = "Сервер запущен по адресу: " + ipAddress.ToString() + ":8005";
                    await Task.Run(async () =>
                    {
                        while (true)
                        {
                            Socket handler = socket.Accept();
                            StringBuilder builder = new StringBuilder();
                            int bytes = 0;
                            byte[] data = new byte[256];

                            do
                            {
                                bytes = handler.Receive(data);
                                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                            }
                            while (handler.Available > 0);
                            Action action = () =>
                            {
                                LogPanel.Text += DateTime.Now.ToShortTimeString() + ": " + builder.ToString() + "\n";
                            };
                            await LogPanel.Dispatcher.BeginInvoke(action);
                        }
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async void Send_Command()
        {

            if (IP.Text == "" || Command.Text == "")
            {
                MessageBox.Show("(2) Убедитесь что вы ввели все значения");
                return;
            }

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    socket.Connect(IPAddress.Parse(IP.Text), 8005);
                    string message = Command.Text;
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    socket.Send(data);

                    data = new byte[256];
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;

                    do
                    {
                        bytes = socket.Receive(data, data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (socket.Available > 0);
                    Action action = () =>
                    {
                        LogPanel.Text += DateTime.Now.ToShortTimeString() + ": " + builder.ToString() + "\n";
                    };
                    await LogPanel.Dispatcher.BeginInvoke(action);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            Command.Text = "";
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
    }
}
