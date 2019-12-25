// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Configuration;
using System.Windows.Media;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public RoutedEventHandler[] methods =
        {
            new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                foreach (var item in Application.Current.Windows)
                {
                    if((item as Window).Name == "settings")
                    {
                        var combobox = m3md2.WinHelper.FindChild<ComboBox>(item as Window, "ColorChoose");
                        var appSettings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        foreach (var u2 in appSettings.AppSettings.Settings)
                        {
                            if ((u2 as KeyValueConfigurationElement).Key == "ColorTheme")
                            {
                                (u2 as KeyValueConfigurationElement).Value = combobox.Text;
	                        }
	                    }
                        appSettings.Save(ConfigurationSaveMode.Minimal);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
	            }
            })
        };

        public Settings()
        {
            InitializeComponent();
            SolidColorBrush brush = new SolidColorBrush(m3md2.ColorThemes.GetColors(ConfigurationManager.AppSettings.Get("ColorTheme"))[0]);
            Grid.Background = brush;
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
                Apply.Click += methods[items.IndexOf(sender as TreeViewItem)];
            }
            catch (Exception)
            {
                MessageBox.Show("(5) Возможно вы уже выбрали этот элемент");
            }
        }
    }
}