// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.IO;
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
        List<UIElement> args = new List<UIElement>();
        string argtype = null;
        IP[] arr;
        public ulong TotalMem = new Memory().GetAllMemory();

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) MainWindow загружен\r\n";
                InfinityListenMenuItem.IsChecked = Convert.ToBoolean(m3md2.StaticVariables.Windows.InfinityListen);
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Режим вечной прослушки установлен на {InfinityListenMenuItem.IsChecked}\r\n";
                SolidColorBrush brush = new SolidColorBrush(m3md2.ColorThemes.GetColors(m3md2.StaticVariables.Settings.ColorTheme)[0]);
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Кисть brush настроена на {brush.ToString()} / {brush.Color.ToString()} (из цветовой темы {m3md2.StaticVariables.Settings.ColorTheme})\r\n";
                SolidColorBrush brush1 = new SolidColorBrush(m3md2.ColorThemes.GetColors(m3md2.StaticVariables.Settings.ColorTheme)[1]);
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Кисть brush настроена на {brush1.ToString()} / {brush1.Color.ToString()} (из цветовой темы {m3md2.StaticVariables.Settings.ColorTheme})\r\n";
                SolidColorBrush brush2 = new SolidColorBrush(m3md2.ColorThemes.GetColors(m3md2.StaticVariables.Settings.ColorTheme)[2]);
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Кисть brush настроена на {brush2.ToString()} / {brush2.Color.ToString()} (из цветовой темы {m3md2.StaticVariables.Settings.ColorTheme})\r\n";
                Grid.Background = brush;
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Grid.Background этого окна установлен на brush\r\n";
                ScrollLog.Background = brush1;
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) ScrollLog.Background этого окна установлен на brush1\r\n";
                foreach (var label in m3md2.WinHelper.FindVisualChildren<Label>(Grid as DependencyObject))
                {
                    label.Foreground = brush2;
                }
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Все элементы Label этого окна: Foreground установлен на brush2\r\n";
                foreach (var textBlock in m3md2.WinHelper.FindVisualChildren<TextBlock>(Grid as DependencyObject))
                {
                    textBlock.Foreground = brush2;
                }
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Все элементы Textblock этого окна: Foreground устаовлен на brush2\r\n";
                foreach (var scrollViewer in m3md2.WinHelper.FindVisualChildren<ScrollViewer>(Grid as DependencyObject))
                {
                    scrollViewer.Foreground = brush2;
                }
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Все элементы ScrollViewer этого окна: Foreground установлен на brush2\r\n";
                DispatcherTimer dispatcherTimer = new DispatcherTimer();
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Установлен таймер для обновления счетчиков производительности\r\n";
                dispatcherTimer.Tick += DispatcherTimer_Tick;
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Установлено событие DispatcherTimer_Tick\r\n";
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Задержка таймера установлена на 1 секунду\r\n";
                dispatcherTimer.Start();
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Таймер запущен\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var CPUnow = myAppCPU.NextValue();
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / DispatcherTimer_Tick event) Значение CPUnow установлено на {CPUnow}\r\n";
                var RAMnow = myAppRAM.NextValue();
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / DispatcherTimer_Tick event) Значение RAMnow установлено на {RAMnow}\r\n";
                CPULabel.Content = $"CPU = {CPUnow}%";
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / DispatcherTimer_Tick event) Значение CPULabel для этого окна установлено на {CPULabel.Content}\r\n";
                RAMLabel.Content = $"RAM = {RAMnow / (1024f * 1024f)}MB / {TotalMem / (1024f * 1024f)}MB";
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / DispatcherTimer_Tick event) Значение RAMLabel для этого окна установлено на {RAMLabel.Content}\r\n";
                TMGLabel.Content = $"Программа работает {(int)myAppTMG.NextValue()}с";
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / DispatcherTimer_Tick event / TimeManagment) Программа работает уже {TMGLabel.Content}\r\n";
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

                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / DispatcherTimer_Tick event) Обновлен CPURectangle (визуальная часть отображения CPUnow)\r\n";
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
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / DispatcherTimer_Tick event) Обновлен RAMRectangle (визуальная часть отображения RAMnow)\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
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
                        IPSet iPSet = new IPSet(true);
                        iPSet.Show();
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Запущено IDSet окно\r\n";
                        return;
                    }
                }
                string showcommand = Command.Text;
                string command = Command.Text;
                foreach (var item in args)
                {
                    if (item is TextBox)
                    {
                        showcommand += " " + (item as TextBox).Text;
                        command += "^" + (item as TextBox).Text;
                    }
                }
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Заданная команда {command} / {showcommand}\r\n";
                Message message = new Message()
                {
                    command = command,
                    ids = ipsall ? arr.Where(x => x.nameofpc != string.Empty).Select(x => x.id.ToString()).ToArray() : ips
                };
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Создан экземпляр Message: Команда {message.command} / ID {message.ids}\r\n";
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Отправлен ApiRequest.Create(message) на messages\r\n";
                Uri res = await ApiRequest.CreateProductAsync(message, "messages");
                LogPanel.Text += $"({DateTime.Now.ToLongTimeString()}) Команда {showcommand} (id: {await ApiRequest.GetProductAsync<uint>("api/v1/messages") - 1}) отправлена.\r\n";
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event to MainWindow.LogPanel) Команда {command} / {showcommand} отправлена.\r\n";
                ListenInfo();
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / Send_Command event) Обновлена информация API\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
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
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
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
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
            }
        }

        private async void Formloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow loaded event) Форма загружена\r\n";
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWIndow loaded event) Отправлен ApiRequest.Delete на api/v1/messages\r\n";
                _ = await ApiRequest.DeleteProductsAsync("api/v1/messages");
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow loaded event) Отправлен ApiRequest.Delete на api/v1/responses\r\n";
                _ = await ApiRequest.DeleteProductsAsync("api/v1/responses");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
            }
        }

        bool CanListen = true;

        private async void StartListenAsync()
        {
            try
            {
                CanListen = true;
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow StartListen method) Значение CanListen установлено на {CanListen}\r\n";
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow StartListen method) Запускаю прослушку клиетов\r\n";
                await Task.Run(() => ListenClients());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow StartListen method) Запускаю прослушку ответов\r\n";
                await Task.Run(() => ListenResponses());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
            }
        }

        private void StopListen()
        {
            try
            {
                CanListen = false;
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow StorListen method) Зачение CanListen установлено на {CanListen}\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
            }
        }

        private async void ListenClients()
        {
            try
            {
                while (true)
                {
                    if (!CanListen)
                    {
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenClients) Обнаружено что CanListen false, прослушка останавливается...\r\n";
                        return;
                    }
                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenClients) Отправлен ApiRequest.Get на api/v1/client\r\n";
                    arr = await ApiRequest.GetProductAsync<IP[]>("/api/v1/client");
                    if (arr == null)
                    {
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenClients) Что-то пошло не так и API вернула null, продолжаем прослушку...\r\n";
                        continue;
                    }
                    string res = "";
                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenClients) Запускаю перебор всех клиентов\r\n";
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].nameofpc == "")
                        {
                            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenClients) Клиент с id {i} отключился от сети, мы не включаем его в список\r\n";
                            continue;
                        }
                        res += "Id: " + arr[i].id + " - " + arr[i].nameofpc + "\n";
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenClients) Итерация {i}: клиент {arr[i].nameofpc} добавлен в список\r\n";
                    }
                    await ClientList.Dispatcher.BeginInvoke(new Action(() => ClientList.Text = res));
                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenClients) Полю ClientList.Text этого окна задано на {res}\r\n";
                    ListenInfo();
                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenClients) Обновлена информация API\r\n";
                    Thread.Sleep(7000);
                }
            }
            catch (Exception ex)
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenClients) Перезапускаю прослушку клиентов...\r\n";
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
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenResponses) Обнаружено, что CanListen false. Останавливаю прослушку...\r\n";
                        return;
                    }
                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenResponses) Отправлен ApiRequest.Get на api/v1/responses\r\n";
                    Responses[] responses = await ApiRequest.GetProductAsync<Responses[]>("api/v1/responses");
                    if (responses == null)
                    {
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenResponses) Что-то пошло не так и API вернула null, продолжаю прослушку...\r\n";
                        continue;
                    }
                    if (isFirstIter)
                    {
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenResponses) Это первая итерация прослушки\r\n";
                        isFirstIter = false;
                        for (int i = 0; i < responses.Length; i++)
                        {
                            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenResponses) В массив responses добавляется ApiRequest.Get по api/v1/responses/{responses[i].id}\r\n";
                            vars.Add(await ApiRequest.GetProductAsync<uint>($"api/v1/responses/{responses[i].id}") - 1);
                        }
                    }
                    if (vars.Count != responses.Length)
                    {
                        for (int i = vars.Count; i < responses.Length; i++)
                        {
                            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenResponses) В ответах системы обнаружен новый ответ, добавляем в массив\r\n";
                            vars.Add(await ApiRequest.GetProductAsync<uint>($"api/v1/responses/{responses[i].id}") - 1);
                        }
                    }
                    for (int i = 0; i < responses.Length; i++)
                    {
                        uint var1 = await ApiRequest.GetProductAsync<uint>($"api/v1/responses/{responses[i].id}") - 1;
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenResponses) Переменная var1 равна {var1}\r\n";
                        if (var1 == vars[i])
                        {
                            m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenResponses) Обнаружен новый ответ от клиента. Подгатавливаем его вывод...\r\n";
                            Response response = await ApiRequest.GetProductAsync<Response>($"api/v1/responses/{responses[i].id}/{var1}");
                            if (response != null)
                            {
                                await LogPanel.Dispatcher.BeginInvoke(new Action(() => LogPanel.Text += "(" + DateTime.Now.ToLongTimeString() + ") " + response.response + "\n"));
                                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenResponses) К LogPanel этого окна добавлено ({DateTime.Now.ToLongTimeString()}) {response.response}\r\n";
                                ListenInfo();
                                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenResponses) Обновлена информация API\r\n";
                                vars[i] = vars[i] + 1;
                                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenResponses) Следующий ответ будет под id {vars[i]}\r\n";
                            }
                        }
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow ListenResponses) Перезапускаю прослушку ответов...\r\n";
                await Task.Run(() => ListenResponses());
            }
        }

        private void IPSetButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / IDSetButton_Click event) Запускаю откно задачи ID для отправки комманд\r\n";
                IPSet iPSet = new IPSet();
                iPSet.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
            }
        }
        bool isSendBlocked;
        private void Command_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow CommandHelper) Набирается команда, проверяю на совпадение\r\n";
                for (int i = 0; i < Arguments.arguments.Length; i++)
                {
                    if (Command.Text == Arguments.arguments[i].Command)
                    {
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
                        return;
                    }
                }
                ClearCommands();
                if (isSendBlocked)
                {
                    SendButton.IsEnabled = true;
                    Command.KeyDown += Command_KeyDown;
                    isSendBlocked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
            }
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
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
            }
        }

        private async void Button_Click1(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in m3md2.WinHelper.FindVisualChildren<Button>(Grid))
                {
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
                                    foreach (var window in Application.Current.Windows)
                                    {
                                        if (window is IPSet)
                                        {
                                            (window as IPSet).isSendFrom = false;
                                        }
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
                                    MessageBox.Show(ex.ToString());
                                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                                    m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                                    m3md2.StaticVariables.Diagnostics.ExceptionCount++;
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
                                    foreach (var window in Application.Current.Windows)
                                    {
                                        if (window is IPSet)
                                        {
                                            (window as IPSet).isSendFrom = false;
                                        }
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
                                    MessageBox.Show(ex.ToString());
                                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                                    m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                                    m3md2.StaticVariables.Diagnostics.ExceptionCount++;
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
		                default:
                            break;
	                }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
            }
        }

        private void Main_Closed(object sender, EventArgs e)
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Главное окно закрывается...\r\n";
                foreach (var window in Application.Current.Windows)
                {
                    if ((window as Window) != null)
                    {
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow) Закрываю окно {window.ToString()}\r\n";
                        (window as Window).Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / CommandsInfo_Click event) Открываю окно настроек приложения\r\n";
                Settings settings = new Settings();
                settings.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
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
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
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
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
            }
        }

        private void MenuItem_Checked(object sender, RoutedEventArgs e) // infinity listener
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / MenuItem_inflistener_Unchecked event) Запускаю прослушку...\r\n";
                StartListenAsync();
                var appSettings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / MenuItem_inflistener_Unchecked event) Получаем нынешнюю конфигурацию приложения\r\n";
                foreach (var u2 in appSettings.AppSettings.Settings)
                {
                    if ((u2 as KeyValueConfigurationElement).Key == "InfnityListen")
                    {
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / MenuItem_inflistener_Unchecked event) Найден ключ InfinityListen и устанавливаем его значение на true\r\n";
                        (u2 as KeyValueConfigurationElement).Value = "True";
                    }
                }
                appSettings.Save(ConfigurationSaveMode.Minimal);
                ConfigurationManager.RefreshSection("appSettings");
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / MenuItem_inflistener_Unchecked event) Конфигурация сохранена и обновлена\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
            }
        }

        private void MenuItem_Unchecked(object sender, RoutedEventArgs e) // infinity listener
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / MenuItem_inflistener_Unchecked event) Останавливаю прослушку...\r\n";
                StopListen();
                var appSettings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / MenuItem_inflistener_Unchecked event) Получаем нынешнюю конфигурацию приложения\r\n";
                foreach (var u2 in appSettings.AppSettings.Settings)
                {
                    if ((u2 as KeyValueConfigurationElement).Key == "InfnityListen")
                    {
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / MenuItem_inflistener_Unchecked event) Найден ключ InfinityListen и устанавливаем его значение на false\r\n";
                        (u2 as KeyValueConfigurationElement).Value = "False";
                    }
                }
                appSettings.Save(ConfigurationSaveMode.Minimal);
                ConfigurationManager.RefreshSection("appSettings");
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(MainWindow / MenuItem_inflistener_Unchecked event) Конфигурация сохранена и обновлена\r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
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
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
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
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
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
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
            }
        }

        private void AppInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("BotNet Server UI.exe\nВерсия 1.6.0 beta 3\nИсходный код/сообщить об ошибке: https://github.com/Zerumi/BotNet-Server/", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Exception) В программе возникло исключение {ex.Message} / {ex.InnerException} ({ex.HResult}) Подробнее в разделе \"Диагностика\"\r\n";
                m3md2.StaticVariables.Diagnostics.exceptions.Add(ex);
                m3md2.StaticVariables.Diagnostics.ExceptionCount++;
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
    }
}