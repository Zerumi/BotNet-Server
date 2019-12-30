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
                if (item.Name == name)
                {
                    colors = item.Colors;
                }
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
                    Color.FromRgb(0,0,0) // font color
                }
            },
            new ColorTheme
            {
                Name = "Pinkerity",
                Colors = new Color[]
                {
                    Color.FromRgb(255,171,214), // main color
                    Color.FromRgb(171,255,177), // second color
                    Color.FromRgb(255,0,128) // font color
                }
            },
            new ColorTheme
            {
                Name = "Hackerman",
                Colors = new Color[]
                {
                    Color.FromRgb(158,240,146), // main color
                    Color.FromRgb(235,255,232), // second color
                    Color.FromRgb(0,0,0) // font color
                }
            }
        };
    }
}
