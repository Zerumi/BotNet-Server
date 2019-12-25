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
            Color[] colors = null;
            foreach (var item in colorthemes)
            {
                if (item.name == name)
                {
                    colors = item.colors;
                }
            }
            return colors;
        }
        static List<ColorTheme> colorthemes = new List<ColorTheme>()
        {
            new ColorTheme
            {
                name = "Standard",
                colors = new Color[]
                {
                    SystemColors.InfoColor,
                    Color.FromRgb(255,255,255),
                    Color.FromRgb(0,0,0)
                }
            },
            new ColorTheme
            {
                name = "Pinkerity",
                colors = new Color[]
                {
                    Color.FromRgb(247,0,206),
                    Color.FromRgb(0,255,238),
                    Color.FromRgb(255,255,255)
                }
            },
            new ColorTheme
            {
                name = "Hackerman",
                colors = new Color[]
                {
                    Color.FromRgb(91,247,0),
                    Color.FromRgb(0,255,238),
                    Color.FromRgb(255,255,255)
                }
            }
        };
    }
}
