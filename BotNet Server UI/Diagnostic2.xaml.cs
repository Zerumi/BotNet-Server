// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для Diagnostic2.xaml
    /// </summary>
    public partial class Diagnostic2 : Window
    {
        public Diagnostic2()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(new Action(async() =>
            {
                while (true)
                {
                    await LogPanel.Dispatcher.BeginInvoke(new Action(() => LogPanel.Text = m3md2.StaticVariables.Diagnostics.ProgramInfo + "// Обнавляется каждую секунду."));
                    Thread.Sleep(1000);
                }
            }));
        }
    }
}
