using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Microsoft.Xna.Framework.Graphics;

namespace MonoXAML
{
    public static class Utility
    {
        private const int GRADIENT_RESOLUTION = 128;
        
        public static Texture2D GradientToTexture(GradientBrush gradient)
        {
            if(gradient is LinearGradientBrush linearGradient)
            {
                return LinearGradientToTexture(linearGradient);
            }
            else
            {
                throw new NotImplementedException("Cannot render " + gradient.GetType());
            }
        }
        private static Texture2D LinearGradientToTexture(LinearGradientBrush linearGradient)
        {
            Texture2D texture = new Texture2D(
                    XAMLManager.GraphicsDeviceManager.GraphicsDevice, 
                    GRADIENT_RESOLUTION, 
                    GRADIENT_RESOLUTION);

            Microsoft.Xna.Framework.Vector2 startPoint = linearGradient.StartPoint.ToVector() * GRADIENT_RESOLUTION;
            Microsoft.Xna.Framework.Vector2 endPoint = linearGradient.EndPoint.ToVector() * GRADIENT_RESOLUTION;

            float gradientDistance = Microsoft.Xna.Framework.Vector2.Distance(startPoint, endPoint);
            
            Microsoft.Xna.Framework.Color[] colors = new Microsoft.Xna.Framework.Color[GRADIENT_RESOLUTION * GRADIENT_RESOLUTION];

            for (int i = 0; i < colors.Length; i++)
            {
                int x = i % GRADIENT_RESOLUTION;
                int y = (int)Math.Floor((double)i / (double)GRADIENT_RESOLUTION);

                float perpendicularDistance = PerpendicularDistance(startPoint, endPoint, new Microsoft.Xna.Framework.Vector2(x, y));
                
                colors[i] = GetColor(perpendicularDistance / gradientDistance, linearGradient);
            }

            texture.SetData(colors);

            return texture;
        }
        private static Microsoft.Xna.Framework.Color GetColor(float offset, GradientBrush brush)
        {
            GradientStop left = null, right = null;

            foreach (GradientStop stop in brush.GradientStops)
            {
                if(stop.Offset <= offset)
                {
                    left = stop;
                }

                if(stop.Offset >= offset)
                {
                    right = stop;
                    break;
                }
            }

            return Microsoft.Xna.Framework.Color.Lerp(left.Color.Convert(), right.Color.Convert(), (float)GetLocalDelta(offset, left.Offset, right.Offset));
        }
        /// <summary>
        /// Returns point on line segment betwee <paramref name="a"/> & <paramref name="b"/> perpendicular to <paramref name="p"/>
        /// </summary>
        public static Microsoft.Xna.Framework.Vector2 LineSegmentPoint(Microsoft.Xna.Framework.Vector2 a, Microsoft.Xna.Framework.Vector2 b, Microsoft.Xna.Framework.Vector2 p)
        {
            float lineLength = Microsoft.Xna.Framework.Vector2.DistanceSquared(a, b);

            //a == b
            if (lineLength == 0)
                return p;

            float dot = Microsoft.Xna.Framework.Vector2.Dot(p - a, b - a);
            float t = Math.Max(0, Math.Min(1, dot / lineLength));

            return new Microsoft.Xna.Framework.Vector2()
            {
                X = a.X + t * (b.X - a.X),
                Y = a.Y + t * (b.Y - a.Y),
            };
        }
        /// <summary>
        /// Returns the distance between <paramref name="a"/> and <paramref name="b"/> using <paramref name="p"/> as the perpendicular midway point
        /// </summary>
        /// <returns>Distance between <paramref name="a"/> and line segment point</returns>
        private static float PerpendicularDistance(Microsoft.Xna.Framework.Vector2 a, Microsoft.Xna.Framework.Vector2 b, Microsoft.Xna.Framework.Vector2 p)
        {
            return Microsoft.Xna.Framework.Vector2.Distance(a, LineSegmentPoint(a, b, p));
        }
        private static double GetLocalDelta(double globalOffset, double leftOffset, double rightOffset)
        {
            if (rightOffset == leftOffset)
                return 0;

            return (globalOffset - leftOffset) / (rightOffset - leftOffset);
        }
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
