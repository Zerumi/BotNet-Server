// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Configuration;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

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
                        m3md2.StaticVariables.Settings.ColorTheme = combobox.Text;
                        MessageBox.Show("Для применения изменений программа будет перезапущена без ввода пароля", "Настройки", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                        Close_Settings(m3md2.WinHelper.FindChild<Grid>(item as Window, "Grid"));
                        m3md2.StaticVariables.Diagnostics.ProgramInfo = "";
                        Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                        foreach (var window in Application.Current.Windows)
                        {
                            if ((window as MainWindow) != null)
                            {
                                (window as MainWindow).Close();
                            }
                        }
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
                    }
                }
            }),
            new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                foreach (var item in Application.Current.Windows)
                {
                    if((item as Window).Name == "settings")
                    {
                        var checkbox = m3md2.WinHelper.FindChild<CheckBox>(item as Window, "CheckThis");
                        var checkbox1 = m3md2.WinHelper.FindChild<CheckBox>(item as Window, "Expect100Continue");
                        var appSettings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        foreach (var u2 in appSettings.AppSettings.Settings)
                        {
                            if ((u2 as KeyValueConfigurationElement).Key == "IgnoreBigLog")
                            {
                                (u2 as KeyValueConfigurationElement).Value = checkbox.IsChecked.GetValueOrDefault().ToString();
                            }
                            else if ((u2 as KeyValueConfigurationElement).Key == "Expect100Continue")
                            {
                                (u2 as KeyValueConfigurationElement).Value = (!checkbox1.IsChecked.GetValueOrDefault()).ToString();
                            }
                        }
                        appSettings.Save(ConfigurationSaveMode.Minimal);
                        ConfigurationManager.RefreshSection("appSettings");
                        m3md2.StaticVariables.Settings.IgnoreBigLog = checkbox.IsChecked.GetValueOrDefault();
                        Close_Settings(m3md2.WinHelper.FindChild<Grid>(item as Window, "Grid"));
                    }
                }
            }),
            new RoutedEventHandler((object sender, RoutedEventArgs e) => {
                foreach (var item in Application.Current.Windows)
                {
                    if((item as Window).Name == "settings")
                    {
                        var mineweb = m3md2.WinHelper.FindChild<TextBox>(item as Window, "ApiChoose");
                        var appSettings = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        foreach (var u2 in appSettings.AppSettings.Settings)
                        {
                            if ((u2 as KeyValueConfigurationElement).Key == "MineWebUri")
                            {
                                (u2 as KeyValueConfigurationElement).Value = mineweb.Text;
                            }
                        }
                        appSettings.Save(ConfigurationSaveMode.Minimal);
                        ConfigurationManager.RefreshSection("appSettings");
                        Close_Settings(m3md2.WinHelper.FindChild<Grid>(item as Window, "Grid"));

                        MessageBoxResult result = MessageBox.Show("Чтобы изменения вступили в силу, программу нужно перезагрузить. Сделать это сейчас?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            System.Windows.Forms.Application.Restart();

                            System.Windows.Application.Current.Shutdown();
                        }   
                        else
                        {
                            return;
                        }
                    }
                }
            })
        };

        Action[] loadmethods = new Action[]
        {
            new Action(() => {

            }),
            new Action(() => {

            }),
            new Action(() => {
                foreach (var item in Application.Current.Windows)
                {
                    if((item as Window).Name == "settings")
                    {
                        var mineweb = m3md2.WinHelper.FindChild<TextBox>(item as Window, "ApiChoose");
                        mineweb.Text = ConfigurationManager.AppSettings.Get("MineWebUri");
                    }
                }
            })
        };

        private static void Close_Settings(Grid Grid)
        {
        linkforech:
            foreach (UIElement control in Grid.Children)
            {
                if (Grid.GetColumn(control) == 1)
                {
                    if ((control as Button) != null)
                    {
                        if ((control as Button).Name == "Apply")
                        {
                            (control as Button).Visibility = Visibility.Hidden;
                            continue;
                        }
                    }
                    Grid.Children.Remove(control);
                    goto linkforech;
                }
            }
        }

        public Settings()
        {
            InitializeComponent();
            SolidColorBrush brush = new SolidColorBrush(m3md2.ColorThemes.GetColors(ConfigurationManager.AppSettings.Get("ColorTheme"))[0]);
            SolidColorBrush brush1 = new SolidColorBrush(m3md2.ColorThemes.GetColors(m3md2.StaticVariables.Settings.ColorTheme)[1]);
            SettingsView.Background = brush1;
            Grid.Background = brush;
        }

        readonly List<TreeViewItem> items = new List<TreeViewItem>();

        private void Setting_Load(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < m3md2_startup.Settings.SettingsList.Length; i++)
            {
                items.Add(new TreeViewItem()
                {
                    Foreground = new SolidColorBrush(m3md2.ColorThemes.GetColors(m3md2.StaticVariables.Settings.ColorTheme)[2]),
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
                Close_Settings(Grid);
                for (int i = 0; i < m3md2_startup.Settings.settings[items.IndexOf(sender as TreeViewItem)].SettingObjects.Length; i++)
                {
                    var element = m3md2_startup.Settings.settings[items.IndexOf(sender as TreeViewItem)].SettingObjects[i] as UIElement;
                    element.SetValue(Grid.ColumnProperty, 1);
                    if (LogicalTreeHelper.GetParent(element as DependencyObject) != null)
                    {
                        var Parent = (Grid)LogicalTreeHelper.GetParent(element as DependencyObject);
                        Parent.Children.Remove(element);
                    }
                    Grid.Children.Add(element);
                }
                loadmethods[items.IndexOf(sender as TreeViewItem)].Invoke();
                Apply.Visibility = Visibility.Visible;
                foreach (var method in methods)
                {
                    Apply.RemoveHandler(ButtonBase.ClickEvent, method);
                }
                Apply.Click += methods[items.IndexOf(sender as TreeViewItem)];
            }
            catch (Exception ex)
            {
                MessageBox.Show("(5) Возможно вы уже выбрали этот элемент");
                ExceptionHandler.RegisterNew(ex, false);
            }
        }
    }
}