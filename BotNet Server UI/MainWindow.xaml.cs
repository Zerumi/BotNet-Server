// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using CommandsLibrary;
using System.Threading;
using Microsoft.AspNetCore.SignalR.Client;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool ipsall;
        public string[] ips;
        readonly List<UIElement> args = new List<UIElement>();
        readonly string argtype = null;
        Client[] arr;

        readonly SolidColorBrush brush = new SolidColorBrush(m3md2.StaticVariables.Settings.colors[0]);
        readonly SolidColorBrush brush1 = new SolidColorBrush(m3md2.StaticVariables.Settings.colors[1]);
        readonly SolidColorBrush brush2 = new SolidColorBrush(m3md2.StaticVariables.Settings.colors[2]);
        readonly SolidColorBrush brush3 = new SolidColorBrush(m3md2.StaticVariables.Settings.colors[3]);

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                InfinityListenMenuItem.IsChecked = Convert.ToBoolean(m3md2.StaticVariables.Windows.InfinityListen);
                Grid.Background = brush;
                ScrollLog.Background = brush1;
                foreach (var label in m3md2.WinHelper.FindVisualChildren<Label>(Grid))
                {
                    label.Foreground = brush2;
                }
                foreach (var textBlock in m3md2.WinHelper.FindVisualChildren<TextBlock>(Grid))
                {
                    textBlock.Foreground = brush2;
                }
                foreach (var scrollViewer in m3md2.WinHelper.FindVisualChildren<ScrollViewer>(Grid))
                {
                    scrollViewer.Foreground = brush2;
                }
                foreach (var textbox in m3md2.WinHelper.FindVisualChildren<TextBox>(Grid))
                {
                    textbox.Background = brush3;
                    textbox.Foreground = brush2;
                }
                if (Convert.ToBoolean(ConfigurationRequest.GetValueByKey("EnablePerfomanceCounter")))
                {
                    StartPerfomanceCounter();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        readonly DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public void StartPerfomanceCounter()
        {
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        public void StopPerfomanceCounter()
        {
            dispatcherTimer.Tick -= DispatcherTimer_Tick;
            dispatcherTimer.Stop();
            CPULabel.Content = $"Подсчет производительности остановлен";
            RAMLabel.Content = $"Подсчет производительности остановлен";
            TMGLabel.Content = $"Подсчет производительности остановлен";
            SolidColorBrush brush = new SolidColorBrush(Colors.LightGray);
            CPURectangle.Fill = brush;
            RAMRectangle.Fill = brush;
        }

        readonly PerformanceCounter myAppCPU = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName, true);
        readonly PerformanceCounter myAppRAM = new PerformanceCounter("Process", "Working Set", Process.GetCurrentProcess().ProcessName, true);
        readonly PerformanceCounter myAppTMG = new PerformanceCounter("Process", "Elapsed Time", Process.GetCurrentProcess().ProcessName, true);

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                ulong TotalMem = new Memory().GetAllMemory();
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
                    new GradientStop(CPUnow / 100 < 0.8 ? Colors.LightGreen : Colors.Red, CPUnow / 100));
                CPULinearGradientBrush.GradientStops.Add(
                    new GradientStop(Colors.LightGray, CPUnow / 100));
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
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        public async void Send_Command()
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Начата отправка команды\r\n";
                if (Command.Text == "")
                {
                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Ошибка: поле с командой пустое\r\n";
                    MessageBox.Show("(2) Постарайтесь ввести все значения правильно!");
                    return;
                }
                if (!ipsall)
                {
                    bool isArrayValid = false;
                    try
                    {
                        Array.ConvertAll(ips, x => int.Parse(x));
                        isArrayValid = true;
                    }
                    catch (Exception)
                    {
                        if (ips == null)
                        {
                            
                        }
                        else if (ips[0] == "all" || ips[0] == "null")
                        {
                            isArrayValid = true;
                        }
                    }
                    if (ips == null || !isArrayValid)
                    {
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Программа запустилось впервые и не имеет заданного списка ID\r\n";
                        IDSet iPSet = new IDSet(true);
                        iPSet.Show();
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Запущено IDSet окно\r\n";
                        return;
                    }
                }
                string showcommand = Command.Text;
                string command = Command.Text;
                var textargs = args.OfType<TextBox>().ToArray();
                foreach (var item in textargs)
                {
                    showcommand += " " + item.Text;
                    command += "^" + item.Text;
                }
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Заданная команда {command} / {showcommand}\r\n";
                Command message = new Command()
                {
                    command = command,
                    Clients = ipsall ? arr.Where(x => x.nameofpc != string.Empty).ToList() : Array.FindAll(arr, x => Array.Find(ips, y => y == Convert.ToString(x.id)) != null).ToList()
                };
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Создан экземпляр Message: Команда {message.command} / ID {message.Clients}\r\n";
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Отправлен ApiRequest.Create(command) на command\r\n";
                Uri res = await ApiRequest.CreateProductAsync(message, "command");
                LogPanel.Text += $"({DateTime.Now.ToLongTimeString()}) Команда {showcommand} (id: {await ApiRequest.GetProductAsync<uint>($"api/command") - 1}) отправлена.\r\n";
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event to MainWindow.LogPanel) Команда {command} / {showcommand} отправлена.\r\n";
                ListenInfo();
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Обновлена информация API\r\n";
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Send_Command();
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / SendButton_Click event) Нажата кнопка \"Отправить\", вызываю Send_Command\r\n";
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void Command_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    Send_Command();
                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Command_keyDown event) Нажата клавиша Enter в поле для ввода команды/аргумента, вызываю Send_Command\r\n";
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private async void Formloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow loaded event) Форма загружена\r\n";
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWIndow loaded event) Отправлен ApiRequest.Delete на api/command\r\n";
                _ = await ApiRequest.DeleteProductsAsync($"api/command");
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow loaded event) Отправлен ApiRequest.Delete на api/response\r\n";
                _ = await ApiRequest.DeleteProductsAsync($"api/response");
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        CancellationTokenSource cts;

        private async void StartListenAsync()
        {
            try
            {
                cts = new CancellationTokenSource();
                await Task.Run(() => ListenClients());
                await Task.Run(() => ListenResponses());
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void StopListen()
        {
            try
            {
                cts.Cancel();
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow StorListen method) Вызвана операция остановки метода\r\n";
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private async void ListenClients()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private async Task SetClientList()
        {
            string res = "";
            arr.ToList().ForEach(x => res += "Id: " + x.id + " - " + x.nameofpc + "\n");
            await ClientList.Dispatcher.BeginInvoke(new Action(() => ClientList.Text = res));
        }

        private async void ListenResponses()
        {
            try
            {
                var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:64760/signalr/response")
                .Build();

                connection.Closed += async (error) =>
                {
                    ExceptionHandler.RegisterNew(error);
                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    await connection.StartAsync();
                };

                connection.On<Response>("ReceiveResponse", async(response) =>
                {
                    await Task.Run(async () => await LogPanel.Dispatcher.BeginInvoke(new Action(() => LogPanel.Text += "(" + DateTime.Now.ToLongTimeString() + ") id: " + response.id + " / " + response.response + "\n")));
                });

                await connection.StartAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void IPSetButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / IDSetButton_Click event) Запускаю окно задачи ID для отправки комманд\r\n";
                IDSet IDset = new IDSet();
                IDset.Show();
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }
        bool isSendBlocked;

        private void Command_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isCommandWrote = false;
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow CommandHelper) Набирается команда, проверяю на совпадение\r\n";
                ListHelper.Items.Clear();
                for (int i = 0; i < Arguments.arguments.Length; i++)
                {
                    if (Command.Text == Arguments.arguments[i].Command)
                    {
                        isCommandWrote = true;
                        ClearCommands();
                        if (Arguments.arguments[i].IsForServer)
                        {
                            SendButton.IsEnabled = false;
                            Command.KeyDown -= Command_KeyDown;
                            isSendBlocked = true;
                        }
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow CommandHelper) Команда {Command.Text} распознана\r\n";
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow CommandHelper) Тип аргументов {argtype}\r\n";
                        for (int j = 0, k = Arguments.arguments[i].ArgumentCount; j < Arguments.arguments[i].ArgumentCount; j++)
                        {
                            var list = Activator.CreateInstance(Arguments.arguments[i].ArgumentType[j]);

                            (list as FrameworkElement).Name = Arguments.arguments[i].ArgumentsName[j];
                            (list as FrameworkElement).HorizontalAlignment = HorizontalAlignment.Stretch;
                            (list as FrameworkElement).Margin = new Thickness(10 + j * (270 / Arguments.arguments[i].ArgumentCount), 10, 10 + --k * (270 / Arguments.arguments[i].ArgumentCount), 18);

                            if (Arguments.arguments[i].ArgumentType[j] == typeof(Button))
                            {
                                (list as Button).Content = Arguments.arguments[i].ArgumentsList[j];
                                (list as Button).VerticalAlignment = VerticalAlignment.Bottom;
                                (list as Button).Click += new RoutedEventHandler(Button_Click1);
                            }
                            else if (Arguments.arguments[i].ArgumentType[j] == typeof(TextBox))
                            {
                                (list as TextBox).TextWrapping = TextWrapping.NoWrap;
                                (list as TextBox).Background = brush3;
                                (list as TextBox).Foreground = brush2;
                                if (!Arguments.arguments[i].IsForServer)
                                {
                                    (list as TextBox).KeyDown += new KeyEventHandler(Command_KeyDown);
                                }
                            }
                            args.Add(list as UIElement);
                            args[j].SetValue(Grid.ColumnProperty, 2);
                            args[j].SetValue(Grid.RowProperty, 2);
                            Grid.Children.Add(args[j]);
                            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow CommandHelper) В поле с аргументами добавлен {argtype} {Arguments.arguments[i].ArgumentsList[j]}\r\n";
                        }
                    }
                    if (Commands.commands[i].Contains(Command.Text) && Commands.commands[i] != Command.Text)
                    {
                        var templboxitem = new ListBoxItem()
                        {
                            Content = Commands.commands[i]
                        };
                        templboxitem.PreviewMouseDown += Templboxitem_PreviewMouseDown;
                        ListHelper.Visibility = Visibility.Visible;
                        ListHelper.Items.Add(templboxitem);
                    }
                }
                if (ListHelper.Items.Count == 0)
                {
                    ListHelper.Visibility = Visibility.Hidden;
                }
                if (!isCommandWrote)
                {
                    ClearCommands();
                    if (isSendBlocked)
                    {
                        SendButton.IsEnabled = true;
                        Command.KeyDown += Command_KeyDown;
                        isSendBlocked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void Templboxitem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Command.Text = Convert.ToString((sender as ListBoxItem).Content);
            ListHelper.Visibility = Visibility.Hidden;
        }

        private async void ListenInfo()
        {
            try
            {
            linkinfo:
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenInfo) Отправлен ApiRequest.Get на api\r\n";
                var Info = await ApiRequest.GetProductAsync<Info>("/api");
                if (Info == null)
                {
                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenInfo) Что-то пошло не так и Api вернула null, повторяю запрос...\r\n";
                    goto linkinfo;
                }
                await InfoBlock.Dispatcher.BeginInvoke(new Action(() => InfoBlock.Text = "Подключено к " + ApiRequest.BaseAddress + "\nПорт: " + Info.port + "\nAPI версии " + Info.version + " / Среда разработки " + Info.environment + "\nВсего клиентов: " + Info.clients + " / Всего сообщений: " + Info.messages));
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenInfo) Обновляю InfoBlock данного окна на полученный экземпляр класса Info\r\n";
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private async void Button_Click1(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = e.Source as Button;
                switch (item.Name)
                {
                    case "screen_OpenPanel":
                        {
                            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ScreenClick event) Открываю окно списка клиентов со скриншотами\r\n";
                            Screens screenwindow = new Screens();
                            screenwindow.Show();
                            break;
                        }
                    case "update_Download":
                        {
                            try
                            {
                                IDSet[] idset = Application.Current.Windows.OfType<IDSet>().ToArray();
                                foreach (var window in idset)
                                {
                                    window.isSendFrom = false;
                                }
                                Command.IsEnabled = false;
                                SendButton.IsEnabled = false;
                                item.IsEnabled = false;
                                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow DownloadClick event) Открываю окно загрузки файла\r\n";
                                byte[] fileContent = new byte[0];
                                var filePath = string.Empty;
                                var fileName = string.Empty;
                                using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
                                {
                                    openFileDialog.InitialDirectory = "c:\\";
                                    openFileDialog.Filter = "All files (*.*)|*.*";
                                    openFileDialog.FilterIndex = 1;
                                    openFileDialog.RestoreDirectory = true;

                                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        //Get the path of specified file
                                        filePath = openFileDialog.FileName;
                                        fileName = openFileDialog.FileName.Split('\\').LastOrDefault();
                                        //Read the bytes
                                        fileContent = File.ReadAllBytes(filePath);
                                    }
                                }
                                List<byte[]> btlist = new List<byte[]>();
                                int value = 0;
                                for (int i = 0; i < Math.Ceiling(fileContent.Length / 20000d); i++)//bt это исходный массив байтов
                                {
                                    if (fileContent.Length - value > 20000)
                                    {
                                        byte[] btt = new byte[20000];
                                        btlist.Add(btt);
                                        Array.ConstrainedCopy(fileContent, value, btlist[i], 0, 20000);//копирует элементы одного массива в другой, с указанием начального индекса и количества элементов для копирования
                                        value += 20000;//счетчик, сколько байтов уже отсчитано
                                    }
                                    else
                                    {
                                        byte[] btt = new byte[fileContent.Length - value];
                                        btlist.Add(btt);
                                        Array.ConstrainedCopy(fileContent, value, btlist[i], 0, fileContent.Length - value);
                                    }
                                }
                                bool isFirstIter = true;
                                int j = 0;
                                item.Content = $"Загрузить файл ({j++}/{btlist.Count})";
                                item.IsEnabled = true;
                                foreach (var bytearray in btlist)
                                {
                                    if (isFirstIter)
                                    {
                                        UpdateFile file = new UpdateFile()
                                        {
                                            filename = fileName,
                                            filebytes = bytearray
                                        };
                                        _ = await ApiRequest.CreateProductAsync(file, "update");
                                        isFirstIter = false;
                                        item.Content = $"Загрузить файл ({j++}/{btlist.Count})";
                                        continue;
                                    }
                                    UpdateFile file1 = new UpdateFile()
                                    { filebytes = bytearray };
                                    _ = await ApiRequest.CreateProductAsync(file1, "nextupdate");
                                    item.Content = $"Загрузить файл ({j++}/{btlist.Count})";
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.RegisterNew(ex);
                            }
                            finally
                            {
                                item.Content = "Загрузить файл";
                                item.IsEnabled = true;
                                Command.IsEnabled = true;
                                SendButton.IsEnabled = true;
                            }
                            break;
                        }
                    case "mines_add_Download":
                        {
                            try
                            {
                                IDSet[] idset = Application.Current.Windows.OfType<IDSet>().ToArray();
                                foreach (var window in idset)
                                {
                                    window.isSendFrom = false;
                                }
                                Command.IsEnabled = false;
                                SendButton.IsEnabled = false;
                                item.IsEnabled = false;
                                m3md2.WinHelper.FindChild<TextBox>(Grid, "mines_add_FilePath").IsEnabled = false;
                                byte[] fileContent = new byte[0];
                                var filePath = string.Empty;
                                var fileName = string.Empty;
                                using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
                                {
                                    openFileDialog.InitialDirectory = "c:\\";
                                    openFileDialog.Filter = "All files (*.*)|*.*";
                                    openFileDialog.FilterIndex = 1;
                                    openFileDialog.RestoreDirectory = true;

                                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        //Get the path of specified file
                                        filePath = openFileDialog.FileName;
                                        fileName = openFileDialog.FileName.Split('\\').LastOrDefault();
                                        //Read the bytes
                                        fileContent = File.ReadAllBytes(filePath);
                                    }
                                }
                                fileContent = File.ReadAllBytes(filePath);
                                List<byte[]> btlist = new List<byte[]>();
                                int value = 0;
                                for (int i = 0; i < Math.Ceiling(fileContent.Length / 20000d); i++)//bt это исходный массив байтов
                                {
                                    if (fileContent.Length - value > 20000)
                                    {
                                        byte[] btt = new byte[20000];
                                        btlist.Add(btt);
                                        Array.ConstrainedCopy(fileContent, value, btlist[i], 0, 20000);//копирует элементы одного массива в другой, с указанием начального индекса и количества элементов для копирования
                                        value += 20000;//счетчик, сколько байтов уже отсчитано
                                    }
                                    else
                                    {
                                        byte[] btt = new byte[fileContent.Length - value];
                                        btlist.Add(btt);
                                        Array.ConstrainedCopy(fileContent, value, btlist[i], 0, fileContent.Length - value);
                                    }
                                }
                                var spath = m3md2.WinHelper.FindChild<TextBox>(Grid, "mines_add_FilePath").Text;
                                _ = await UpdateCenterRequest.DeleteProductAsync($"{spath}");
                                int j = 0;
                                foreach (var item1 in btlist)
                                {
                                    _ = await UpdateCenterRequest.CreateProductAsync(item1, spath);
                                    item.Content = $"Загрузить файл ({++j}/{btlist.Count})";
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.RegisterNew(ex);
                            }
                            finally
                            {
                                item.Content = "Загрузить файл";
                                item.IsEnabled = true;
                                Command.IsEnabled = true;
                                SendButton.IsEnabled = true;
                                m3md2.WinHelper.FindChild<TextBox>(Grid, "mines_add_FilePath").IsEnabled = true;
                            }
                            break;
                        }
                    case "mines_remove_bRemove":
                        {
                            var spath = m3md2.WinHelper.FindChild<TextBox>(Grid, "mines_remove_FilePath").Text;
                            var code = await UpdateCenterRequest.DeleteProductAsync($"{spath}");
                            MessageBox.Show($"Запрос на удаление завершен с кодом {code}");
                            break;
                        }
                    case "screendemo_opendemos":
                        {
                            //m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ScreenClick event) Открываю окно списка клиентов с текущей демонстрацией экрана\r\n";
                            //Demos demoswindow = new Demos();
                            //demoswindow.Show();
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void Main_Closed(object sender, EventArgs e)
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Главное окно закрывается...\r\n";
                Array.ForEach(Application.Current.Windows.OfType<Window>().ToArray(), x => x.Close());
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / CommandsInfo_Click event) Открываю окно настроек приложения\r\n";

                if (!(Application.Current.Windows.OfType<Settings>().FirstOrDefault()?.Activate()).GetValueOrDefault(false))
                {
                    Settings settings = new Settings();
                    settings.Show();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void CommadsInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / CommandsInfo_Click event) Запускаю окно помощи по командам...\r\n";
                CommandsInfo commands = new CommandsInfo();
                commands.Show();
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) // open folder
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / MenuItemOpenFolder_Click event) Открываю проводник с основной директорией этого приложения\r\n";
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / MenuItemOpenFolder_Click event) Открывается директория {AppDomain.CurrentDomain.BaseDirectory}\r\n";
                Process.Start("explorer.exe", AppDomain.CurrentDomain.BaseDirectory);
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void MenuItem_Checked(object sender, RoutedEventArgs e) // infinity listener
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / MenuItem_inflistener_Unchecked event) Запускаю прослушку...\r\n";
                StartListenAsync();
                ConfigurationRequest.WriteValueByKey("InfnityListen", "True");
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / MenuItem_inflistener_Unchecked event) Конфигурация сохранена и обновлена\r\n";
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void MenuItem_Unchecked(object sender, RoutedEventArgs e) // infinity listener
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / MenuItem_inflistener_Unchecked event) Останавливаю прослушку...\r\n";
                StopListen();
                ConfigurationRequest.WriteValueByKey("InfnityListen", "False");
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / MenuItem_inflistener_Unchecked event) Конфигурация сохранена и обновлена\r\n";
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void Diagnostic1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow Diagnostic1_Click event) Открываю окно показа всех исключений приложения...\r\n";
                Diagnostic1 diagnostic1 = new Diagnostic1();
                diagnostic1.Show();
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void Diagnostic2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow Diagnostic2_Click event) Открываю окно показа всех действий приложения\r\n";
                Diagnostic2 diagnostic2 = new Diagnostic2();
                diagnostic2.Show();
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void Diagnostic3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow Diagnoctic3_Click event) Запускаю окно диагностики API\r\n";
                Diagnostic3 diagnostic3 = new Diagnostic3();
                diagnostic3.Show();
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void AppInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("BotNet Server UI.exe by Zerumi\n" +
                    "Версия 2.0.0 beta 18\n" +
                    "Основное API BotNet (https://botnet-api.glitch.me/)\n" +
                    "Версия ASP.NET Release 1" +
                    "Исходный код/сообщить об ошибке: https://github.com/Zerumi/BotNet-Server/ \n" +
                    "Discord: Zerumi#4666", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void ClearCommands()
        {
            for (int i = 0; i < args.Count; i++)
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow CommandHelper) Удаляю {args[i]} так как команда была стерта, а также стираю массив\r\n";
                Grid.Children.Remove(args[i]);
            }
            args.Clear();
        }

        private void ChangeServer_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите изменить сервер? (Программа будет перезагружена)", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                System.Windows.Forms.Application.Restart();

                System.Windows.Application.Current.Shutdown();
            }
            else
            {
                return;
            }
        }

        private void Command_LostFocus(object sender, RoutedEventArgs e)
        {
            ListHelper.Visibility = Visibility.Hidden;
        }

        private void Command_GotFocus(object sender, RoutedEventArgs e)
        {
            Command_TextChanged(sender, new TextChangedEventArgs(e.RoutedEvent, UndoAction.None));
        }

        private async void GetMessage_Click(object sender, RoutedEventArgs e)
        {
            _ = MessageBox.Show(await ApiRequest.GetProductAsync<string>($"api/support/version_note/{Assembly.GetExecutingAssembly().GetName().Version}"));
        }
    }
}