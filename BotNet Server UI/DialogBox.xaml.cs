// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace BotNet_Server_UI
{
    partial class DialogBox : Window
    {
        public DialogBox()
        {
            InitializeComponent();
        }

        public string ResponseText
        {
            get {
                if (cbShow.IsChecked.GetValueOrDefault())
                {
                    return sResponseTextBox.Text;
                }
                return ResponseTextBox.Password; 
            }
            set {
                if (cbShow.IsChecked.GetValueOrDefault())
                {
                    sResponseTextBox.Text = value;
                }
                ResponseTextBox.Password = value;
            }
        }

        public string ServerText
        {
            get { return ServerTextBox.Text; }
            set { ServerTextBox.Text = value; }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void Field_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Load();
            }
        }

        private async void Load()
        {
            try
            {
                AuthButton.IsEnabled = false;
                ResponseTextBox.KeyDown -= Field_KeyDown;
                sResponseTextBox.KeyDown -= Field_KeyDown;
                ApiRequest.BaseAddress = ServerText;
                AuthButton.Content = "Подключение...";
                ApiRequest.ApiVersion = (await ApiRequest.GetProductAsync<Info>("/api")).version;
                UpdateCenterRequest.BaseAddress = ConfigurationRequest.GetValueByKey("MineWebUri");
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Authorization) Запускаю проверку пароля\r\n";
                AuthButton.Content = "Проверка...";
                if (await ApiRequest.GetProductAsync<bool>($"api/v{ApiRequest.ApiVersion}/admin/{ResponseText}"))
                {
                    ConfigurationRequest.WriteValueByKey("MainUri", ServerText);
                    AuthButton.Content = "Загрузка сборок...";
                    if (await CheckDll())
                    {
                        AuthButton.Content = "Загрузка...";
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Authorization) Пароль правильный, запускаю основное окно\r\n";
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        System.Windows.Application.Current.MainWindow.Close();
                    }
                }
                else
                {
                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Authorization) Пароль неправильный, возвращаю окно в исходное положение\r\n";
                    MessageBox.Show("Неправильный пароль");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
            finally
            {
                AuthButton.Content = "ОК";
                AuthButton.IsEnabled = true;
                ResponseTextBox.KeyDown += Field_KeyDown;
                sResponseTextBox.KeyDown += Field_KeyDown;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ResponseTextBox.Visibility = Visibility.Collapsed;
                sResponseTextBox.Visibility = Visibility.Visible;
                sResponseTextBox.Text = ResponseTextBox.Password;
                sResponseTextBox.Focus();
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void cbShow_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                sResponseTextBox.Visibility = Visibility.Collapsed;
                ResponseTextBox.Visibility = Visibility.Visible;
                ResponseTextBox.Password = sResponseTextBox.Text;

                ResponseTextBox.Focus();
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ServerText = ConfigurationRequest.GetValueByKey("MainUri");
        }

        public async Task<bool> CheckDll()
        {
            try
            {
                _ = Assembly.Load("m3md2");
                _ = Assembly.Load("m3md2_startup");
                _ = Assembly.Load("CommandsLibrary");
                AuthButton.Content = "Проверка...";
                VerifyVersion version = await ApiRequest.GetProductAsync<VerifyVersion>($"api/v{ApiRequest.ApiVersion}/support/versions/{Assembly.GetExecutingAssembly().GetName().Version}");
                if (version.isDeprecated)
                {
                    throw new NotSupportedException("Данная версия программы устарела. Пожалуйста, загрузите новую версию в разделе Releases");
                }
                else if (version.isUpdateNeeded)
                {
                    System.Windows.MessageBox.Show("Данная версия программы скоро устареет. Пожалуйста, загрузите новую версию программы в разделе Releases", "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }
                else if (version.isNotLatest)
                {
                    System.Windows.MessageBox.Show("Доступно обновление для этой программы, которое содержит множество исправлений и улучшений, вы можете загрузить его в разделе Releases", "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
                var cmdlib = CommandsLibrary.Verifier.Verify();
                var m3md = m3md2.Verifier.Verify();
                var m3md_startup = m3md2_startup.Verifier.Verify();
                if (!(version.cmdlib.Contains(cmdlib.Item2) && version.m3md2.Contains(m3md.Item2) && version.m3md2_startup.Contains(m3md_startup.Item2)))
                {
                    throw new PlatformNotSupportedException("Данная версия библиотеки не поддерживается этим экземпляром оболочки");
                }
            }
            catch (NotSupportedException ex)
            {
                System.Windows.MessageBox.Show(ex.ToString() + "\nОшибки возникшие во время запуска не позволяют продолжать бесперебойную работу программы.\nУстраните эти ошибки прежде чем начать использование программы");
                Environment.Exit(0);
                return false;
            }
            catch (Exception ex)
            {
                ExceptionHandler.RegisterNew(ex);
                return false;
            }
            return true;
        }
    }
}
