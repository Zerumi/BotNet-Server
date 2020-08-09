// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для IPSet.xaml
    /// </summary>
    public partial class IDSet : Window
    {
        public bool isSendFrom = false;

        SolidColorBrush brush = new SolidColorBrush(m3md2.ColorThemes.GetColors(m3md2.StaticVariables.Settings.ColorTheme)[0]);
        SolidColorBrush brush2 = new SolidColorBrush(m3md2.ColorThemes.GetColors(m3md2.StaticVariables.Settings.ColorTheme)[2]);
        SolidColorBrush brush3 = new SolidColorBrush(m3md2.ColorThemes.GetColors(m3md2.StaticVariables.Settings.ColorTheme)[3]);

        public IDSet()
        {
            InitializeComponent();
            tb_ask.Foreground = brush2;
            tb_ask.Background = brush;
            ResponseTextBox.Background = brush3;
            ResponseTextBox.Foreground = brush2;
        }
        public IDSet(bool isSendFrom)
        {
            this.isSendFrom = isSendFrom;
            InitializeComponent();
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ResponseTextBox.Text == "null")
            {
                if (isSendFrom)
                {
                    MessageBox.Show("(4) Полю недопустимо быть пустым в данном контексте");
                    return;
                }
                ((MainWindow)Application.Current.Windows[0]).ipsall = false;
                ((MainWindow)Application.Current.Windows[0]).ips = null;
                ((MainWindow)Application.Current.Windows[0]).ID_Status.Dispatcher.BeginInvoke(new Action(() => ((MainWindow)Application.Current.Windows[0]).ID_Status.Content = "ID не заданы"));
            }
            else if (ResponseTextBox.Text == "all")
            {
                ((MainWindow)Application.Current.Windows[0]).ipsall = true;
                ((MainWindow)Application.Current.Windows[0]).ips = null;
                ((MainWindow)Application.Current.Windows[0]).ID_Status.Dispatcher.BeginInvoke(new Action(() => ((MainWindow)Application.Current.Windows[0]).ID_Status.Content = "Заданы все ID"));
            }
            else
            {
                if (isSendFrom && string.IsNullOrWhiteSpace(ResponseTextBox.Text))
                {
                    MessageBox.Show("(4) Полю недопустимо быть пустым в данном контексте");
                    return;
                }
                ((MainWindow)Application.Current.Windows[0]).ipsall = false;
                ((MainWindow)Application.Current.Windows[0]).ips = ResponseTextBox.Text.Split(' ');
                ((MainWindow)Application.Current.Windows[0]).ID_Status.Dispatcher.BeginInvoke(new Action(() => ((MainWindow)Application.Current.Windows[0]).ID_Status.Content = "Заданные ID: " + ResponseTextBox.Text));
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
                    if (isSendFrom)
                    {
                        MessageBox.Show("(4) Полю недопустимо быть пустым в данном контексте");
                        return;
                    }
                    ((MainWindow)Application.Current.Windows[0]).ipsall = false;
                    ((MainWindow)Application.Current.Windows[0]).ips = null;
                    ((MainWindow)Application.Current.Windows[0]).ID_Status.Dispatcher.BeginInvoke(new Action(() => ((MainWindow)Application.Current.Windows[0]).ID_Status.Content = "ID не заданы"));
                }
                else if (ResponseTextBox.Text == "all")
                {
                    ((MainWindow)Application.Current.Windows[0]).ipsall = true;
                    ((MainWindow)Application.Current.Windows[0]).ips = null;
                    ((MainWindow)Application.Current.Windows[0]).ID_Status.Dispatcher.BeginInvoke(new Action(() => ((MainWindow)Application.Current.Windows[0]).ID_Status.Content = "Заданы все ID"));
                }
                else
                {
                    if (isSendFrom && string.IsNullOrWhiteSpace(ResponseTextBox.Text))
                    {
                        MessageBox.Show("(4) Полю недопустимо быть пустым в данном контексте");
                        return;
                    }
                    ((MainWindow)Application.Current.Windows[0]).ipsall = false;
                    ((MainWindow)Application.Current.Windows[0]).ips = ResponseTextBox.Text.Split(' ');
                    ((MainWindow)Application.Current.Windows[0]).ID_Status.Dispatcher.BeginInvoke(new Action(() => ((MainWindow)Application.Current.Windows[0]).ID_Status.Content = "Заданные ID: " + ResponseTextBox.Text));
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
