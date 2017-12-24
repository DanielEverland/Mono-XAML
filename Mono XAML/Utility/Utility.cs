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
            return new Microsoft.Xna.Framework.Rectangle(GetRectPosition(element), GetRectSize(element));
        }
        public static Microsoft.Xna.Framework.Point GetRectSize(System.Windows.FrameworkElement element)
        {
            return new Microsoft.Xna.Framework.Point()
            {
                X = GetRectWidth(element),
                Y = GetRectHeight(element),
            };
        }
        public static Microsoft.Xna.Framework.Point GetRectPosition(System.Windows.FrameworkElement element)
        {
            return new Microsoft.Xna.Framework.Point()
            {
                X = GetRectXPosition(element),
                Y = GetRectYPosition(element),
            };
        }
        public static int GetRectWidth(System.Windows.FrameworkElement element)
        {
            if (double.IsNaN(element.Width) && element.HorizontalAlignment == System.Windows.HorizontalAlignment.Stretch)
            {
                return (int)(XAMLManager.GraphicsDeviceManager.PreferredBackBufferWidth - (element.Margin.Left + element.Margin.Right));
            }
            else
            {
                return (int)element.Width;
            }
        }
        public static int GetRectHeight(System.Windows.FrameworkElement element)
        {
            if (double.IsNaN(element.Height) && element.VerticalAlignment == System.Windows.VerticalAlignment.Stretch)
            {
                return (int)(XAMLManager.GraphicsDeviceManager.PreferredBackBufferHeight - (element.Margin.Top + element.Margin.Bottom));
            }
            else
            {
                return (int)element.Height;
            }
        }
        public static int GetRectYPosition(System.Windows.FrameworkElement element)
        {
            switch (element.VerticalAlignment)
            {
                case System.Windows.VerticalAlignment.Top:
                    return (int)element.Margin.Top;
                case System.Windows.VerticalAlignment.Center:
                    return (int)(XAMLManager.GraphicsDeviceManager.PreferredBackBufferHeight / 2 + element.Margin.Top - element.Height / 2);
                case System.Windows.VerticalAlignment.Bottom:
                    return (int)(XAMLManager.GraphicsDeviceManager.PreferredBackBufferHeight - (element.Margin.Bottom + element.Height));
                case System.Windows.VerticalAlignment.Stretch:
                    {
                        if (double.IsNaN(element.Height))
                        {
                            return (int)element.Margin.Top;
                        }
                        else
                        {
                            return (int)(XAMLManager.GraphicsDeviceManager.PreferredBackBufferHeight / 2 - element.Height / 2);                        
                        }
                    }                    
                default:
                    break;
            }

            throw new ArgumentException("Cannot identify alignment " + element.VerticalAlignment);
        }
        public static int GetRectXPosition(System.Windows.FrameworkElement element)
        {
            switch (element.HorizontalAlignment)
            {
                case System.Windows.HorizontalAlignment.Left:
                    return (int)element.Margin.Left;
                case System.Windows.HorizontalAlignment.Center:
                    return (int)(XAMLManager.GraphicsDeviceManager.PreferredBackBufferWidth / 2 + element.Margin.Left - element.Width / 2);
                case System.Windows.HorizontalAlignment.Right:
                    return (int)(XAMLManager.GraphicsDeviceManager.PreferredBackBufferWidth - (element.Margin.Right + element.Width));
                case System.Windows.HorizontalAlignment.Stretch:
                    {
                        if (double.IsNaN(element.Width))
                        {
                            return (int)element.Margin.Left;
                        }
                        else
                        {
                            return (int)(XAMLManager.GraphicsDeviceManager.PreferredBackBufferWidth / 2 + element.Margin.Left - element.Width / 2);
                        }
                    }                    
            }

            throw new ArgumentException("Cannot identify alignment " + element.HorizontalAlignment);
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
