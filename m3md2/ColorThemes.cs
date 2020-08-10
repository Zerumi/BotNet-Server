// This code is licensed under the isc license. You can improve the code by keeping this comments 
// (or by any other means, with saving authorship by Zerumi and PizhikCoder retained)
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace m3md2
{
    public static class ColorThemes
    {
        /// <summary>
        /// Получает цветовую тему по ее имени
        /// </summary>
        /// <param name="name">Название цветовой темы</param>
        /// <returns>Массив цветов этой темы</returns>
        public static Color[] GetColors(string name)
        {
            Color[] colors = colorthemes.Find(x => x.Name == name).Colors;
            if (colors == default)
            {
                MessageBox.Show("Тема " + name + " не была найдена. Вероятно она была удалена из программы. Последняя удаленная тема: Pinkerity");
                colors = new Color[] { Color.FromRgb(255, 255, 255), Color.FromRgb(255, 255, 255), Color.FromRgb(255, 255, 255), Color.FromRgb(255, 255, 255) };
            }
            return colors;
        }
        static readonly List<ColorTheme> colorthemes = new List<ColorTheme>()
        {
            new ColorTheme
            {
                Name = "Standard",
                Colors = new Color[]
                {
                    SystemColors.InfoColor, // main color
                    Color.FromRgb(255,255,255), // second color
                    Color.FromRgb(0,0,0), // font color
                    Color.FromRgb(255,255,255) // third color
                }
            },
            new ColorTheme
            {
                Name = "Hackerman",
                Colors = new Color[]
                {
                    Color.FromRgb(66,255,91), // main color
                    Color.FromRgb(66,230,255), // second color
                    Color.FromRgb(0,0,0), // font color
                    Color.FromRgb(148,255,66) // third color
                }
            },
            new ColorTheme
            {
                Name = "Dark",
                Colors = new Color[]
                {
                    Color.FromRgb(47,49,54), // main color
                    Color.FromRgb(54,57,63), // second color
                    Color.FromRgb(227,225,230), // font color
                    Color.FromRgb(64,68,75) // third color
                }
            }
        };
    }
}
