// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CommandsLibrary;

namespace BotNet_Server_UI
{
    /// <summary>
    /// Логика взаимодействия для CommandsInfo.xaml
    /// </summary>
    public partial class CommandsInfo : Window
    {
        public CommandsInfo()
        {
            InitializeComponent();
            SolidColorBrush brush = new SolidColorBrush(m3md2.ColorThemes.GetColors(m3md2.StaticVariables.Settings.ColorTheme)[0]);
            SolidColorBrush brush1 = new SolidColorBrush(m3md2.ColorThemes.GetColors(m3md2.StaticVariables.Settings.ColorTheme)[1]);
            SolidColorBrush brush2 = new SolidColorBrush(m3md2.ColorThemes.GetColors(m3md2.StaticVariables.Settings.ColorTheme)[2]);
            Grid.Background = brush;
            CommandsPanel.Background = brush1;
            foreach (var label in m3md2.WinHelper.FindVisualChildren<Label>(Grid as DependencyObject))
            {
                label.Foreground = brush2;
            }
            foreach (var textBlock in m3md2.WinHelper.FindVisualChildren<TextBlock>(Grid as DependencyObject))
            {
                textBlock.Foreground = brush2;
            }
            foreach (var scrollViewer in m3md2.WinHelper.FindVisualChildren<ScrollViewer>(Grid as DependencyObject))
            {
                scrollViewer.Foreground = brush2;
            }
        }

        private void Label_Click(object sender, MouseButtonEventArgs e)
        {
            IArgument argument = Array.Find(CommandsLibrary.Arguments.arguments, x => x.Command == (sender as Label).Content.ToString());
            Command.Content = argument.Command;
            Arguments.Content = $"Кол-во аргументов: {argument.ArgumentCount}";
            ArgListBox.Text = "";
            if (argument.ArgumentCount != 0)
            {
                for (int i = 0; i < argument.ArgumentsList.Length; i++)
                {
                    ArgListBox.Text += argument.ArgumentsList[i] + " - " + argument.ArgumentType[i].Name + "\n";
                }
            }
            CmdDescription.Text = argument.CommandInfo;
        }

        readonly List<Label> labels = new List<Label>();

        private void CommInf_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Commands.commands.Length; i++)
            {
                labels.Add(new Label()
                {
                    Foreground = new SolidColorBrush(m3md2.ColorThemes.GetColors(m3md2.StaticVariables.Settings.ColorTheme)[2]),
                    Content = Commands.commands[i]
                });
                labels[i].PreviewMouseDown += Label_Click;
                CommandsPanel.Children.Add(labels[i]);
            }
        }
    }
}
