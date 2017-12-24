using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MonoXAML
{
    public static class Utility
    {
        public static Microsoft.Xna.Framework.Rectangle GetRectangle(System.Windows.FrameworkElement element)
        {
            return new Microsoft.Xna.Framework.Rectangle((int)element.Margin.Left, (int)element.Margin.Top, (int)element.Width, (int)element.Height);
        }
        public static Microsoft.Xna.Framework.Rectangle GetRectangle(System.Windows.Rect rect)
        {
            return new Microsoft.Xna.Framework.Rectangle((int)rect.TopLeft.X, (int)rect.TopLeft.Y, (int)rect.Width, (int)rect.Height);
        }
        public static Microsoft.Xna.Framework.Color GetColor(Brush brush)
        {
            byte r = ((Color)brush.GetValue(SolidColorBrush.ColorProperty)).R;
            byte g = ((Color)brush.GetValue(SolidColorBrush.ColorProperty)).G;
            byte b = ((Color)brush.GetValue(SolidColorBrush.ColorProperty)).B;
            byte a = ((Color)brush.GetValue(SolidColorBrush.ColorProperty)).A;

            return new Microsoft.Xna.Framework.Color(r, g, b, a);
        }
    }
}
