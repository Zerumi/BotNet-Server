// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System.Windows;
using System.Windows.Controls;
using System.Configuration;

namespace m3md2_startup
{
    public class Settings
    {
        public string Setting { get; set; }
        public object[] SettingObjects { get; set; }
        public static string[] SettingsList =
        {
            "Цветовая тема"
        };
        public static Settings[] settings =
        {
            new Settings()
            {
                Setting = "Цветовая тема",
                SettingObjects = new object[]
                {
                    new Label()
                    {
                        Content = "Выберите цветовую тему:",
                        Margin = new Thickness(35,40,35,40),
                        FontSize = 11
                    },
                    new ComboBox()
                    {
                        Name = "ColorChoose",
                        ItemsSource = new string[]
                        {
                            "Standard",
                            "Pinkerity",
                            "Hackerman"
                        },
                        SelectedIndex = 0,
                        Margin = new Thickness(40,65,240,315)
                    }
                }
            }
        };
    }
}
