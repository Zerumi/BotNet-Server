// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
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
            get { return ResponseTextBox.Text; }
            set { ResponseTextBox.Text = value; }
        }

        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Authorization) Запускаю проверку пароля\r\n";
                AuthButton.IsEnabled = false;
                if (await ApiRequest.GetProductAsync<bool>($"api/v1/admin/{ResponseText}"))
                {
                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Authorization) Пароль правильный, запускаю основное окно\r\n";
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    System.Windows.Application.Current.MainWindow.Close();
                }
                else
                {
                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Authorization) Пароль неправильный, возвращаю окно в исходное положение\r\n";
                    MessageBox.Show("Неправильный пароль");
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
                AuthButton.IsEnabled = true;
            }
        }

        private async void Field_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Authorization) Запускаю проверку пароля\r\n";
                    AuthButton.IsEnabled = false;
                    if (await ApiRequest.GetProductAsync<bool>($"api/v1/admin/{ResponseText}"))
                    {
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Authorization) Пароль правильный, запускаю основное окно\r\n";
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        System.Windows.Application.Current.MainWindow.Close();
                    }
                    else
                    {
                        m3md2.StaticVariables.Diagnostics.ProgramInfo += $"{DateTime.Now.ToLongTimeString()}(Authorization) Пароль неправильный, возвращаю окно в исходное положение\r\n";
                        MessageBox.Show("Неправильный пароль");
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
            finally
            {
                AuthButton.IsEnabled = true;
            }
        }
    }
}
