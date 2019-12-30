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
using System.Configuration;
using CommandsLibrary;

//             m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}() \n";
// экземпляр ввода информации в статистическое поле
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
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) MainWindow загружен\n";
            InfinityListenMenuItem.IsChecked = Convert.ToBoolean(m3md2.StaticVariables.Windows.InfinityListen);
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Режим вечной прослушки установлен на {InfinityListenMenuItem.IsChecked}\n";
            SolidColorBrush brush = new SolidColorBrush(m3md2.ColorThemes.GetColors(m3md2.StaticVariables.Settings.ColorTheme)[0]);
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Кисть brush настроена на {brush.ToString()} / {brush.Color.ToString()} (из цветовой темы {m3md2.StaticVariables.Settings.ColorTheme})\n";
            SolidColorBrush brush1 = new SolidColorBrush(m3md2.ColorThemes.GetColors(m3md2.StaticVariables.Settings.ColorTheme)[1]);
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Кисть brush настроена на {brush1.ToString()} / {brush1.Color.ToString()} (из цветовой темы {m3md2.StaticVariables.Settings.ColorTheme})\n";
            SolidColorBrush brush2 = new SolidColorBrush(m3md2.ColorThemes.GetColors(m3md2.StaticVariables.Settings.ColorTheme)[2]);
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Кисть brush настроена на {brush2.ToString()} / {brush2.Color.ToString()} (из цветовой темы {m3md2.StaticVariables.Settings.ColorTheme})\n";
            Grid.Background = brush;
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Grid.Background этого окна установлен на brush\n";
            ScrollLog.Background = brush1;
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) ScrollLog.Background этого окна установлен на brush1\n";
            foreach (var label in m3md2.WinHelper.FindVisualChildren<Label>(Grid as DependencyObject))
            {
                label.Foreground = brush2;
            }
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Все элементы Label этого окна: Foreground установлен на brush2\n";
            foreach (var textBlock in m3md2.WinHelper.FindVisualChildren<TextBlock>(Grid as DependencyObject))
            {
                textBlock.Foreground = brush2;
            }
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Все элементы Textblock этого окна: Foreground устаовлен на brush2\n";
            foreach (var scrollViewer in m3md2.WinHelper.FindVisualChildren<ScrollViewer>(Grid as DependencyObject))
            {
                scrollViewer.Foreground = brush2;
            }
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Все элементы ScrollViewer этого окна: Foreground установлен на brush2\n";
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Установлен таймер для обновления счетчиков производительности\n";
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Установлено событие DispatcherTimer_Tick\n";
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Задержка таймера установлена на 1 секунду\n";
            dispatcherTimer.Start();
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Таймер запущен\n";
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var CPUnow = myAppCPU.NextValue();
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / DispatcherTimer_Tick event) Значение CPUnow установлено на {CPUnow}\n";
            var RAMnow = myAppRAM.NextValue();
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / DispatcherTimer_Tick event) Значение RAMnow установлено на {RAMnow}\n";
            CPULabel.Content = $"CPU = {CPUnow}%";
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / DispatcherTimer_Tick event) Значение CPULabel для этого окна установлено на {CPULabel.Content}\n";
            RAMLabel.Content = $"RAM = {RAMnow / (1024f * 1024f)}MB / {TotalMem / (1024f * 1024f)}MB";
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / DispatcherTimer_Tick event) Значение RAMLabel для этого окна установлено на {RAMLabel.Content}\n";
            TMGLabel.Content = $"Программа работает {(int)myAppTMG.NextValue()}с";
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / DispatcherTimer_Tick event / TimeManagment) Программа работает уже {TMGLabel.Content}\n";
            LinearGradientBrush CPULinearGradientBrush =
            new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0)
            };
            CPULinearGradientBrush.GradientStops.Add(
                new GradientStop(CPUnow / 100 < 0.8 ? Colors.LightGreen : Colors.Red, CPUnow / 100));
            CPULinearGradientBrush.GradientStops.Add(
                new GradientStop(Colors.LightGray, CPUnow / 100));
            CPURectangle.Fill = CPULinearGradientBrush;

            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / DispatcherTimer_Tick event) Обновлен CPURectangle (визуальная часть отображения CPUnow)\n";
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
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / DispatcherTimer_Tick event) Обновлен RAMRectangle (визуальная часть отображения RAMnow)\n";
        }

        public async void Send_Command()
        {
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Начата отправка команды\n";
            if (Command.Text == "")
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Ошибка: поле с командой пустое\n";
                MessageBox.Show("(2) Постарайтесь ввести все значения правильно!");
                return;
            }
            if (!ipsall)
            {
                if (ips == null)
                {
                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Программа запустилось впервые и не имеет заданного списка ID\n";
                    IPSet iPSet = new IPSet(true);
                    iPSet.Show();
                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Запущено IDSet окно\n";
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
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Заданная команда {command} / {showcommand}\n";
                Message message = new Message()
                {
                    command = command,
                    ids = ipsall ? arr.Select(x => x.id.ToString()).ToArray() : ips
                };
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Создан экземпляр Message: Команда {message.command} / ID {message.ids}\n"; 
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Отправлен ApiRequest.Create(message) на messages\n";
                Uri res = await ApiRequest.CreateProductAsync(message, "messages");
                LogPanel.Text += $"({DateTime.Now.ToLongTimeString()}) Команда {showcommand} (id: {await ApiRequest.GetProductAsync<uint>("api/v1/messages") - 1}) отправлена.\n";
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event to MainWindow.LogPanel) Команда {command} / {showcommand} отправлена.\n";
                ListenInfo();
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Обновлена информация API\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) В программе возникло исключение: {ex.Message} / {ex.InnerException} ({ex.HResult})\n";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Send_Command();
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / SendButton_Click event) Нажата кнопка \"Отправить\", вызываю Send_Command\n";
        }

        private void Command_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Send_Command();
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Command_keyDown event) Нажата клавиша Enter в поле для ввода команды/аргумента, вызываю Send_Command\n";
            }
        }

        private async void Formloaded(object sender, RoutedEventArgs e)
        {
            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}() \n";
            _ = await ApiRequest.DeleteProductsAsync("api/v1/messages");
            _ = await ApiRequest.DeleteProductsAsync("api/v1/responses");
        }

        bool CanListen = true;

        private async void StartListenAsync()
        {
            CanListen = true;
            await Task.Run(() => ListenClients());
            await Task.Run(() => ListenResponses());
        }

        private void StopListen()
        {
            CanListen = false;
        }

        private async void ListenClients()
        {
            try
            {
                while (true)
                {
                    if (!CanListen)
                    {
                        return;
                    }
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
                    if (!CanListen)
                    {
                        return;
                    }
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
            foreach (var window in Application.Current.Windows)
            {
                if ((window as Window) != null)
                {
                    (window as Window).Close();
                }
            }
        }

        public void Close_All()
        {
            foreach (var item in Application.Current.Windows)
            {
                (item as Window).Close();
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Show();
        }

        private void CommadsInfo_Click(object sender, RoutedEventArgs e)
        {
            CommandsInfo commands = new CommandsInfo();
            commands.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) // open folder
        {
            Process.Start("explorer.exe", AppDomain.CurrentDomain.BaseDirectory);
        }

        private void MenuItem_Checked(object sender, RoutedEventArgs e) // infinity listener
        {
            StartListenAsync(); 
            var appSettings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (var u2 in appSettings.AppSettings.Settings)
            {
                if ((u2 as KeyValueConfigurationElement).Key == "InfnityListen")
                {
                    (u2 as KeyValueConfigurationElement).Value = "True";
                }
            }
            appSettings.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("appSettings");

        }

        private void MenuItem_Unchecked(object sender, RoutedEventArgs e) // infinity listener
        {
            StopListen(); 
            var appSettings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (var u2 in appSettings.AppSettings.Settings)
            {
                if ((u2 as KeyValueConfigurationElement).Key == "InfnityListen")
                {
                    (u2 as KeyValueConfigurationElement).Value = "False";
                }
            }
            appSettings.Save(ConfigurationSaveMode.Minimal);
            ConfigurationManager.RefreshSection("appSettings");

        }

        private void Diagnostic1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Diagnostic2_Click(object sender, RoutedEventArgs e)
        {
            Diagnostic2 diagnostic2 = new Diagnostic2();
            diagnostic2.Show();
        }

        private void Diagnostic3_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}