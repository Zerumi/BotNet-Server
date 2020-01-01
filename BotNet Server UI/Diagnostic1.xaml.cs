// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
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
    /// Логика взаимодействия для Diagnostic1.xaml
    /// </summary>
    public partial class Diagnostic1 : Window
    {
        public Diagnostic1()
        {
            InitializeComponent();
        }

        List<Button> buttons = new List<Button>();

        private void ExsLoaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < m3md2.StaticVariables.Diagnostics.ExceptionCount; i++)
            {
                buttons.Add(new Button()
                {
                    Name = "Button" + buttons.Count,
                    Content = $"{m3md2.StaticVariables.Diagnostics.exceptions[buttons.Count].Message}"
                });
                buttons[i].Click += Ex_Click;
                Exceptions.Children.Add(buttons[i]);
            }
        }

        private void Ex_Click(object sender, EventArgs e)
        {
            MessageBox.Show(m3md2.StaticVariables.Diagnostics.exceptions[Convert.ToInt32((sender as FrameworkElement).Name.Replace("Button",""))].ToString());
        }
    }
}
