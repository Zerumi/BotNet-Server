// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Linq;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public RoutedEventHandler[] applymethods =
        {
            new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                var item = Application.Current.Windows.OfType<Settings>().First();
                var combobox = m3md2.WinHelper.FindChild<ComboBox>(item, "ColorChoose");
                ConfigurationRequest.WriteValueByKey("ColorTheme", combobox.Text);
                m3md2.StaticVariables.Settings.ColorTheme = combobox.Text;
                m3md2.StaticVariables.Settings.colors = m3md2.ColorThemes.GetColors(combobox.Text);
                MessageBox.Show("Для применения изменений программа будет перезапущена без ввода пароля", "Настройки", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                Close_Settings(m3md2.WinHelper.FindChild<Grid>(item, "Grid"));
                m3md2.StaticVariables.Diagnostics.ProgramInfo = "";
                Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                Application.Current.Windows.OfType<MainWindow>().First().Close();
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            }),
            new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                var item = Application.Current.Windows.OfType<Settings>().First();
                var checkbox = m3md2.WinHelper.FindChild<CheckBox>(item, "CheckThis");
                var checkbox1 = m3md2.WinHelper.FindChild<CheckBox>(item, "Expect100Continue");
                ConfigurationRequest.WriteValueByKey("IgnoreBigLog", checkbox.IsChecked.GetValueOrDefault().ToString());
                ConfigurationRequest.WriteValueByKey("Expect100Continue", (!checkbox1.IsChecked.GetValueOrDefault()).ToString());
                m3md2.StaticVariables.Settings.IgnoreBigLog = checkbox.IsChecked.GetValueOrDefault();
                Close_Settings(m3md2.WinHelper.FindChild<Grid>(item, "Grid"));
            }),
            new RoutedEventHandler((object sender, RoutedEventArgs e) => {
                var item = Application.Current.Windows.OfType<Settings>().First();
                var mineweb = m3md2.WinHelper.FindChild<TextBox>(item, "ApiChoose");
                ConfigurationRequest.WriteValueByKey("MineWebUri", mineweb.Text);
                Close_Settings(m3md2.WinHelper.FindChild<Grid>(item, "Grid"));

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
            }),
            new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {
                var item = Application.Current.Windows.OfType<Settings>().First();
                var checkbox = m3md2.WinHelper.FindChild<CheckBox>(item, "PerfomanceCounter");
                MainWindow _MainWindow = Application.Current.Windows[0] as MainWindow;
                bool epc = Convert.ToBoolean(ConfigurationRequest.GetValueByKey("EnablePerfomanceCounter"));
                if (checkbox.IsChecked.GetValueOrDefault() && !epc)
                {
                    _MainWindow.StartPerfomanceCounter();
                }
                else if (!checkbox.IsChecked.GetValueOrDefault() && epc)
                {
                    _MainWindow.StopPerfomanceCounter();
                }
                ConfigurationRequest.WriteValueByKey("EnablePerfomanceCounter", checkbox.IsChecked.GetValueOrDefault().ToString());
                Close_Settings(m3md2.WinHelper.FindChild<Grid>(item, "Grid"));
            })
        };
        readonly Action[] loadmethods = new Action[]
        {
            new Action(() => {
                var item = Application.Current.Windows.OfType<Settings>().First();
                var colortheme = ConfigurationRequest.GetValueByKey("ColorTheme");
                var combox = m3md2.WinHelper.FindChild<ComboBox>(item, "ColorChoose");
                combox.SelectedIndex = Array.IndexOf(combox.ItemsSource.OfType<string>().ToArray(), Array.Find(combox.ItemsSource.OfType<string>().ToArray(), x => x == colortheme));
            }),
            new Action(() => {
                var item = Application.Current.Windows.OfType<Settings>().First();
                var ibl = Convert.ToBoolean(ConfigurationRequest.GetValueByKey("IgnoreBigLog"));
                var e100c = Convert.ToBoolean(ConfigurationRequest.GetValueByKey("Expect100Continue"));
                var cb_ibl = m3md2.WinHelper.FindChild<CheckBox>(item, "CheckThis");
                var cb_e100c = m3md2.WinHelper.FindChild<CheckBox>(item, "Expect100Continue");
                cb_ibl.IsChecked = ibl;
                cb_e100c.IsChecked = !e100c;
            }),
            new Action(() => {
                var item = Application.Current.Windows.OfType<Settings>().First();
                var mineweb = m3md2.WinHelper.FindChild<TextBox>(item, "ApiChoose");
                mineweb.Text = ConfigurationRequest.GetValueByKey("MineWebUri");
            }),
            new Action(() => {
                var item = Application.Current.Windows.OfType<Settings>().First();
                var epc = Convert.ToBoolean(ConfigurationRequest.GetValueByKey("EnablePerfomanceCounter"));
                var cb_epc = m3md2.WinHelper.FindChild<CheckBox>(item, "PerfomanceCounter");
                cb_epc.IsChecked = epc;
            })
        };

        private static void Close_Settings(Grid Grid)
        {
            Array.Find(Grid.Children.OfType<Button>().ToArray(), x => x.Name == "Apply").Visibility = Visibility.Hidden;

            Array.ForEach(Array.FindAll(Grid.Children.OfType<UIElement>().ToArray(), x => Grid.GetColumn(x) == 1 && !((x as Button)?.Name == "Apply")).ToArray(), y => Grid.Children.Remove(y));
        }

        SolidColorBrush brush = new SolidColorBrush(m3md2.StaticVariables.Settings.colors[0]);
        SolidColorBrush brush1 = new SolidColorBrush(m3md2.StaticVariables.Settings.colors[1]);
        SolidColorBrush brush2 = new SolidColorBrush(m3md2.StaticVariables.Settings.colors[2]);
        SolidColorBrush brush3 = new SolidColorBrush(m3md2.StaticVariables.Settings.colors[3]);

        public Settings()
        {
            InitializeComponent();
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
                    Foreground = new SolidColorBrush(m3md2.StaticVariables.Settings.colors[2]),
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
                    if (element is Label || element is TextBlock || element is CheckBox || element is TextBox)
                    {
                        element.SetValue(ForegroundProperty, brush2);
                    }
                    if (element is TextBox)
                    {
                        element.SetValue(BackgroundProperty, brush3);
                    }
                    if (LogicalTreeHelper.GetParent(element) != null)
                    {
                        var Parent = (Grid)LogicalTreeHelper.GetParent(element);
                        Parent.Children.Remove(element);
                    }
                    Grid.Children.Add(element);
                }
                loadmethods[items.IndexOf(sender as TreeViewItem)].Invoke();
                Apply.Visibility = Visibility.Visible;
                foreach (var method in applymethods)
                {
                    Apply.RemoveHandler(ButtonBase.ClickEvent, method);
                }
                Apply.Click += applymethods[items.IndexOf(sender as TreeViewItem)];
            }
            catch (Exception ex)
            {
                MessageBox.Show("(5) Возможно, вы уже выбрали этот элемент");
                ExceptionHandler.RegisterNew(ex);
            }
        }
    }
}