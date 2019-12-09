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
    /// Логика взаимодействия для Screens.xaml
    /// </summary>
    public partial class Screens : Window
    {
        public Screens()
        {
            InitializeComponent();
        }

        private void ScrForm_Loaded(object sender, RoutedEventArgs e)
        {
            ListScrenns.Items.Add(new Button()
            {

            });
            //for (int i = 0; i < *Length*; i++)
            //{

            //}
        }
    }
}
