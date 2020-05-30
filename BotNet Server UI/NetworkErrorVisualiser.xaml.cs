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
using System.Windows.Threading;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для NetworkErrorVisualiser.xaml
    /// </summary>
    public partial class NetworkErrorVisualiser : Window
    {

        public NetworkErrorVisualiser(int index)
        {
            InitializeComponent();
            this.index = index;
        }

        DispatcherTimer OpacityTimer;
        DispatcherTimer timer;

        int index;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 3)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
            OpacityTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(120)
            };
            OpacityTimer.Tick += OpacityTimer_Tick;
            OpacityTimer.Start();
        }

        private async void OpacityTimer_Tick(object sender, EventArgs e)
        {
            await Task.Run(new Action(() => {
                for (double i = 0.5; i <= 1; i+=0.125)
                {
                    Dispatcher.BeginInvoke(new Action(() => Opacity = i));
                    Thread.Sleep(15);
                }
                for (double i = 1; i >= 0.5; i-=0.125)
                {
                    Dispatcher.BeginInvoke(new Action(() => Opacity = i));
                    Thread.Sleep(15);
                }
            }));

            GC.Collect();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            OpacityTimer.Stop();
            timer.Stop();
            m3md2.StaticVariables.Settings.IsDataProblem[index] = false;
            this.Close();
            GC.Collect();
        }
    }
}
