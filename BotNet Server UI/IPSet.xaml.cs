using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
                ((MainWindow)Application.Current.Windows[0]).ips = false;
                ((MainWindow)Application.Current.Windows[0]).UnblockTextIp();
            }
            if (ResponseTextBox.Text == "all")
            {
                ((MainWindow)Application.Current.Windows[0]).ips = true;
                ((MainWindow)Application.Current.Windows[0]).BlockTextIP();
            }
            else
            {
                ((MainWindow)Application.Current.Windows[0]).ips = true;
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
                    ((MainWindow)Application.Current.Windows[0]).ips = false;
                    ((MainWindow)Application.Current.Windows[0]).UnblockTextIp();
                }
                if (ResponseTextBox.Text == "all")
                {
                    ((MainWindow)Application.Current.Windows[0]).ips = true;
                    ((MainWindow)Application.Current.Windows[0]).BlockTextIP();
                }
                else
                {
                    ((MainWindow)Application.Current.Windows[0]).ips = true;
                    ((MainWindow)Application.Current.Windows[0]).BlockTextIP();
                }

                Close();
            }
        }
    }
}
