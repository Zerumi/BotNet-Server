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
                    ArgListBox.Text += argument.ArgumentsList[i] + " - " + argument.ArgumentType + "\n";
                }
            }
            CmdDescription.Text = argument.CommandInfo;
        }
        List<Label> labels = new List<Label>();

        private void CommInf_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < Commands.commands.Length; i++)
            {
                labels.Add(new Label()
                {
                    Content = Commands.commands[i]
                });
                labels[i].PreviewMouseDown += Label_Click;
                CommandsPanel.Children.Add(labels[i]);
            }
        }
    }
}
