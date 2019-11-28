using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            get { return ResponseTextBox.Text; }
            set { ResponseTextBox.Text = value; }
        }

        private async void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (await ApiRequest.GetProductAsync<bool>($"api/v1/admin/{ResponseText}"))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                System.Windows.Application.Current.MainWindow.Close();
            }
            else
            {
                MessageBox.Show("Неправильный пароль");
            }
        }
    }
}
