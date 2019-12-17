// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using CommandsLibrary;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly PerformanceCounter myAppCPU = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName, true);
        readonly PerformanceCounter myAppRAM = new PerformanceCounter("Process", "Working Set", Process.GetCurrentProcess().ProcessName, true);
        readonly PerformanceCounter myAppTMG = new PerformanceCounter("Process", "Elapsed Time", Process.GetCurrentProcess().ProcessName, true);

        public bool ipsall;
        public string[] ips;
        dynamic[] args = new dynamic[0];
        string argtype = null;
        IP[] arr;
        public ulong TotalMem = new Memory().GetAllMemory();

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var CPUnow = myAppCPU.NextValue();
            var RAMnow = myAppRAM.NextValue();
            CPULabel.Content = $"CPU = {CPUnow}%";
            RAMLabel.Content = $"RAM = {RAMnow / (1024f * 1024f)}MB / {TotalMem / (1024f * 1024f)}MB";
            TMGLabel.Content = $"Программа работает {(int)myAppTMG.NextValue()}с";
            LinearGradientBrush CPULinearGradientBrush =
            new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0)
            };
            CPULinearGradientBrush.GradientStops.Add(
                new GradientStop(CPUnow/100 < 0.8 ? Colors.LightGreen : Colors.Red, CPUnow/100));
            CPULinearGradientBrush.GradientStops.Add(
                new GradientStop(Colors.LightGray, CPUnow/100));
            CPURectangle.Fill = CPULinearGradientBrush;

            LinearGradientBrush RAMLinearGradientBrush =
            new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0)
            };
            RAMLinearGradientBrush.GradientStops.Add(
                new GradientStop(RAMnow / TotalMem < 0.8 ? Colors.LightGreen : Colors.Red, RAMnow / TotalMem));
            RAMLinearGradientBrush.GradientStops.Add(
                new GradientStop(Colors.LightGray, RAMnow / TotalMem));
            RAMRectangle.Fill = RAMLinearGradientBrush;
        }

        public async void Send_Command()
        {
            if (Command.Text == "")
            {
                MessageBox.Show("Постарайтесь ввести все значения правильно!");
                return;
            }
            if (!ipsall)
            {
                if (ips == null)
                {
                    IPSet iPSet = new IPSet(true);
                    iPSet.Show();
                    return;
                }
            }
            try
            {
                string showcommand = Command.Text;
                string command = Command.Text;
                if (argtype == "TextBox")
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        showcommand += " " + args[i].Text;
                        command += "^" + args[i].Text;
                    }
                }
                Message message = new Message()
                {
                    command = command,
                    ids = ipsall ? arr.Select(x => x.id.ToString()).ToArray() : ips
                };
                Uri res = await ApiRequest.CreateProductAsync(message, "messages");
                LogPanel.Text += $"({DateTime.Now.ToLongTimeString()}) Команда {showcommand} (id: {await ApiRequest.GetProductAsync<uint>("api/v1/messages") - 1}) отправлена.\n";
                ListenInfo();
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
                    arr = await ApiRequest.GetProductAsync<IP[]>("/api/v1/client");
                    if (arr == null)
                    {
                        continue;
                    }
                    string res = "";
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].nameofpc == "")
                        {
                            continue;
                        }
                        res += "Id: " + arr[i].id + " - " + arr[i].nameofpc + "\n";
                    }
                    await ClientList.Dispatcher.BeginInvoke(new Action(() => ClientList.Text = res));
                    ListenInfo();
                    Thread.Sleep(7000);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                await Task.Run(() => ListenClients());
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
                            vars.Add(await ApiRequest.GetProductAsync<uint>($"api/v1/responses/{responses[i].id}") - 1);
                        }
                    }
                    if (vars.Count != responses.Length)
                    {
                        for (int i = vars.Count; i < responses.Length; i++)
                        {
                            vars.Add(await ApiRequest.GetProductAsync<uint>($"api/v1/responses/{responses[i].id}") - 1);
                        }
                    }
                    for (int i = 0; i < responses.Length; i++)
                    {
                        uint var1 = await ApiRequest.GetProductAsync<uint>($"api/v1/responses/{responses[i].id}") - 1;
                        if (var1 == vars[i])
                        {
                            Response response = await ApiRequest.GetProductAsync<Response>($"api/v1/responses/{responses[i].id}/{var1}");
                            if (response != null)
                            {
                                await LogPanel.Dispatcher.BeginInvoke(new Action(() => LogPanel.Text += "(" + DateTime.Now.ToLongTimeString() + ") " + response.response + "\n"));
                                ListenInfo();
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
                await Task.Run(() => ListenResponses());
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
                    argtype = Arguments.arguments[i].ArgumentType;
                    if (argtype == "TextBox")
                    {
                        args = new TextBox[Arguments.arguments[i].ArgumentCount];
                    }
                    else if (argtype == "Button")
                    {
                        args = new Button[Arguments.arguments[i].ArgumentCount];
                    }
                    for (int j = 0, k = Arguments.arguments[i].ArgumentCount; j < Arguments.arguments[i].ArgumentCount; j++)
                    {
                        if (Arguments.arguments[i].ArgumentType == "TextBox")
                        {
                            args[j] = new TextBox()
                            {
                                Name = $"Argument{j + 1}",
                                TextWrapping = TextWrapping.NoWrap,
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                Margin = new Thickness(10 + j * (270 / Arguments.arguments[i].ArgumentCount), 10, 10 + --k * (270 / Arguments.arguments[i].ArgumentCount), 18),
                            };
                            args[j].KeyDown += new KeyEventHandler(Command_KeyDown);
                        }
                        else if (Arguments.arguments[i].ArgumentType == "Button")
                        {
                            args[j] = new Button()
                            {
                                Content = Arguments.arguments[i].ArgumentsList[j],
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Bottom,
                                Margin = new Thickness(10, 10, 10, 18)
                            };
                            args[j].Click += new RoutedEventHandler(Button_Click1);
                        }
                        args[j].SetValue(Grid.ColumnProperty, 2);
                        args[j].SetValue(Grid.RowProperty, 2);
                        Grid.Children.Add(args[j]);
                    }
                    return;
                }
            }
            for (int i = 0; i < args.Length; i++)
            {
                Grid.Children.Remove(args[i]);
            }
        }

        private async void ListenInfo()
        {
            linkinfo:
            var Info = await ApiRequest.GetProductAsync<Info>("/api");
            if (Info == null)
            {
                goto linkinfo;
            }
            await InfoBlock.Dispatcher.BeginInvoke(new Action(() => InfoBlock.Text = "Подключено к " + Info.uri + "\nПорт: " + Info.port + "\nAPI версии " + Info.version + " / Среда разработки " + Info.environment + "\nВсего клиентов: " + Info.clients + " / Всего сообщений: " + Info.messages));
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            Screens screenwindow = new Screens();
            screenwindow.Show();
        }

        private void Main_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}