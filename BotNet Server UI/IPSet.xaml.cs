// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System.Windows;
using System.Windows.Input;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для IPSet.xaml
    /// </summary>
    public partial class IPSet : Window
    {
        bool isSendFrom = false;
        public IPSet()
        {
            InitializeComponent();
        }
        public IPSet(bool isSendFrom)
        {
            this.isSendFrom = isSendFrom;
            InitializeComponent();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResponseTextBox.Text == "null")
            {
                ((MainWindow)Application.Current.Windows[0]).ipsall = false;
                ((MainWindow)Application.Current.Windows[0]).ips = null;
            }
            else if (ResponseTextBox.Text == "all")
            {
                ((MainWindow)Application.Current.Windows[0]).ipsall = true;
                ((MainWindow)Application.Current.Windows[0]).ips = null;
            }
            else
            {
                ((MainWindow)Application.Current.Windows[0]).ipsall = false;
                ((MainWindow)Application.Current.Windows[0]).ips = ResponseTextBox.Text.Split(' ');
            }

            if (isSendFrom)
            {
                ((MainWindow)Application.Current.Windows[0]).Send_Command();
            }

            Close();
        }

        private void Field_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ResponseTextBox.Text == "null")
                {
                    ((MainWindow)Application.Current.Windows[0]).ipsall = false;
                    ((MainWindow)Application.Current.Windows[0]).ips = null;
                }
                else if (ResponseTextBox.Text == "all")
                {
                    ((MainWindow)Application.Current.Windows[0]).ipsall = true;
                    ((MainWindow)Application.Current.Windows[0]).ips = null;
                }
                else
                {
                    ((MainWindow)Application.Current.Windows[0]).ipsall = false;
                    ((MainWindow)Application.Current.Windows[0]).ips = ResponseTextBox.Text.Split(' ');
                }

                if (isSendFrom)
                {
                    ((MainWindow)Application.Current.Windows[0]).Send_Command();
                }

                Close();
            }
        }
    }
}
