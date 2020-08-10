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
            "Цветовая тема",
            "Предупреждения",
            "API",
            "Производительность"
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
                            "Hackerman",
                            "Dark"
                        },
                        SelectedIndex = 0,
                        Margin = new Thickness(40,65,240,315)
                    }
                }
            },
            new Settings()
            {
                Setting = "Предупреждения",
                SettingObjects = new object[]
                {
                    new CheckBox()
                    {
                        Name = "CheckThis",
                        Content = "Игнорировать предупреждение о выской длине лога программы",
                        Margin = new Thickness(10,10,10,370)
                    },
                    new CheckBox()
                    {
                        Name = "Expect100Continue",
                        Content = "Игнорировать ожидание (Может помочь, если не работает отправка команд)",
                        FontSize = 10,
                        Margin = new Thickness(10,30,10,350)
                    }
                }
            },
            new Settings()
            {
                Setting = "API",
                SettingObjects = new object[]
                {
                    new Label()
                    {
                        Content = "Введите адрес MinewebAPI:",
                        Margin = new Thickness(35,40,35,40),
                        FontSize = 11
                    },
                    new TextBox()
                    {
                        Name = "ApiChoose",
                        Margin = new Thickness(40,65,140,315)
                    }
                }
            },
            new Settings()
            {
                Setting = "Производительность",
                SettingObjects = new object[]
                {
                    new CheckBox()
                    {
                        Name = "PerfomanceCounter",
                        Content = "Включить счетчик производительности",
                        Margin = new Thickness(10,10,10,370)
                    }
                }
            }
        };
    }
}
