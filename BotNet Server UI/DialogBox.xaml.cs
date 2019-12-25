// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System.Configuration;
using System.Windows;
using System.Windows.Media;

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
            AuthButton.IsEnabled = false;
            if (await ApiRequest.GetProductAsync<bool>($"api/v1/admin/{ResponseText}"))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                System.Windows.Application.Current.MainWindow.Close();
            }
            else
            {
                MessageBox.Show("Неправильный пароль");
                AuthButton.IsEnabled = true;
            }
        }

        private async void Field_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                AuthButton.IsEnabled = false;
                if (await ApiRequest.GetProductAsync<bool>($"api/v1/admin/{ResponseText}"))
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    System.Windows.Application.Current.MainWindow.Close();
                }
                else
                {
                    MessageBox.Show("Неправильный пароль");
                    AuthButton.IsEnabled = true;
                }
            }
        }
    }
}
