// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        readonly List<TreeViewItem> items = new List<TreeViewItem>();

        private void Setting_Load(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < m3md2_startup.Settings.SettingsList.Length; i++)
            {
                items.Add(new TreeViewItem()
                {
                    Header = m3md2_startup.Settings.SettingsList[i]
                });
                items[i].PreviewMouseDown += Settings_PreviewMouseDown;
                SettingsView.Items.Add(items[i]);
            }
        }

        private void Settings_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                for (int i = 0; i < m3md2_startup.Settings.settings[items.IndexOf(sender as TreeViewItem)].SettingObjects.Length; i++)
                {
                    var element = m3md2_startup.Settings.settings[items.IndexOf(sender as TreeViewItem)].SettingObjects[i] as UIElement;
                    element.SetValue(Grid.ColumnProperty, 1);
                    Grid.Children.Add(element);
                }
                Apply.Visibility = Visibility.Visible;
                Apply.Click += m3md2_startup.Settings.methods[items.IndexOf(sender as TreeViewItem)];
            }
            catch (ArgumentException)
            {
                MessageBox.Show("(5) Возможно вы уже выбрали этот элемент");
            }
        }
    }
}
