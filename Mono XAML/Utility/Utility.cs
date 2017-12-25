using System;
using System.Windows.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoXAML
{
    public static class Utility
    {
        public static bool IsSolidColor(Brush brush)
        {
            return brush is SolidColorBrush;
        }
        public static float Lerp(float value, float a, float b)
        {
            return a * value + b * (1 - value);
        }
        private static bool ShouldRenderStroke(int x, int y, int strokeThickness, int width, int height)
        {
            return x < strokeThickness || y < strokeThickness || y > height - strokeThickness || x > width - strokeThickness;
        }
        private static Microsoft.Xna.Framework.Color GetColor(int x, int y, int width, Microsoft.Xna.Framework.Color[] source)
        {
            int i = GetIndex(x, y, width);

            if(i >= source.Length)
            {
                return Microsoft.Xna.Framework.Color.White;
            }
            else
            {
                return source[i];
            }
        }
        public static Texture2D BrushToTexture(Brush brush, Vector2 size)
        {
            if(brush is GradientBrush gradient)
            {
                return GradientToTexture(gradient, size);
            }
            else if(brush is SolidColorBrush solidColor)
            {
                return DefaultTextures.White;
            }
            else
            {
                throw new NotImplementedException("Cannot render " + brush.GetType());
            }
        }
        public static Texture2D GradientToTexture(GradientBrush gradient, Vector2 size)
        {
            if(gradient is LinearGradientBrush linearGradient)
            {
                return LinearGradientToTexture(linearGradient, size);
            }
            else if(gradient is RadialGradientBrush radialGradient)
            {
                return RadialGradientToTexture(radialGradient, size);
            }
            else
            {
                throw new NotImplementedException("Cannot render " + gradient.GetType());
            }
        }
        private static Texture2D RadialGradientToTexture(RadialGradientBrush gradientBrush, Vector2 size)
        {
            Texture2D texture = new Texture2D(
                    XAMLManager.GraphicsDeviceManager.GraphicsDevice,
                    (int)size.X,
                    (int)size.Y);
            
            Microsoft.Xna.Framework.Color[] colors = new Microsoft.Xna.Framework.Color[texture.Width * texture.Height];

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    colors[GetIndex(x, y, texture.Width)]
                        = GetColor(new Vector2(
                            (float)x / (float)texture.Width,
                            (float)y / (float)texture.Height),
                            gradientBrush);
                }
            }

            texture.SetData(colors);

            return texture;
        }
        private static Texture2D LinearGradientToTexture(LinearGradientBrush linearGradient, Vector2 size)
        {
            Texture2D texture = new Texture2D(
                    XAMLManager.GraphicsDeviceManager.GraphicsDevice,
                    (int)size.X,
                    (int)size.Y);
                        
            Microsoft.Xna.Framework.Color[] colors = new Microsoft.Xna.Framework.Color[texture.Width * texture.Height];

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    colors[GetIndex(x, y, texture.Width)]
                        = GetColor(new Vector2(
                            (float)x / (float)texture.Width,
                            (float)y / (float)texture.Height),
                            linearGradient);
                }
            }

            texture.SetData(colors);

            return texture;
        }
        private static int GetIndex(int x, int y, int width)
        {
            return y * width + x;
        }
        /// <summary>
        /// Returns a color based on a XAML brush
        /// </summary>
        /// <param name="position">Specifies position in the space (0, 0) to (1, 0) - out of bounds values will be clamped</param>
        /// <param name="brush">Brush specified from XAML Designer</param>
        public static Microsoft.Xna.Framework.Color GetColor(Vector2 position, Brush brush)
        {
            position = Vector2.Clamp(position, Vector2.Zero, Vector2.One);

            if(brush is GradientBrush gradient)
            {
                return GetColor(position, brush);
            }
            else if(brush is SolidColorBrush solidColor)
            {
                return solidColor.Color.Convert();
            }
            else
            {
                throw new NotImplementedException("Does not recognize " + brush.GetType());
            }
        }
        private static Microsoft.Xna.Framework.Color GetColor(Vector2 position, GradientBrush brush)
        {
            if(brush is LinearGradientBrush linear)
            {
                return GetColor(position, linear);
            }
            else if(brush is RadialGradientBrush radial)
            {
                return GetColor(position, radial);
            }
            else
            {
                throw new NotImplementedException("Does not recognize " + brush.GetType());
            }
        }
        private static Microsoft.Xna.Framework.Color GetColor(Vector2 position, RadialGradientBrush brush)
        {
            Vector2 radial = brush.GradientOrigin.ToVector();
            
            Vector2 delta = radial - position;

            delta.X /= (float)brush.RadiusX;
            delta.Y /= (float)brush.RadiusY;

            return GetColor(delta.Length(), brush);
        }
        private static Microsoft.Xna.Framework.Color GetColor(Vector2 position, LinearGradientBrush brush)
        {
            float offset = PerpendicularDistance(brush.StartPoint.ToVector(), brush.EndPoint.ToVector(), position);

            return GetColor(offset, brush);
        }
        private static Microsoft.Xna.Framework.Color GetColor(float offset, GradientBrush brush)
        {
            GradientStop left = null, right = null;

            foreach (GradientStop stop in brush.GradientStops)
            {
                if (left == null)
                    left = stop;

                if (right == null)
                    right = stop;

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
        public static Vector2 LineSegmentPoint(Vector2 a, Vector2 b, Vector2 p)
        {
            float lineLength = Vector2.DistanceSquared(a, b);

            //a == b
            if (lineLength == 0)
                return p;

            float dot = Vector2.Dot(p - a, b - a);
            float t = Math.Max(0, Math.Min(1, dot / lineLength));

            return new Vector2()
            {
                X = a.X + t * (b.X - a.X),
                Y = a.Y + t * (b.Y - a.Y),
            };
        }
        /// <summary>
        /// Returns the distance between <paramref name="a"/> and <paramref name="b"/> using <paramref name="p"/> as the perpendicular midway point
        /// </summary>
        /// <returns>Distance between <paramref name="a"/> and line segment point</returns>
        private static float PerpendicularDistance(Vector2 a, Vector2 b, Vector2 p)
        {
            return Vector2.Distance(a, LineSegmentPoint(a, b, p));
        }
        private static double GetLocalDelta(double globalOffset, double leftOffset, double rightOffset)
        {
            if (rightOffset == leftOffset)
                return 0;

            return (globalOffset - leftOffset) / (rightOffset - leftOffset);
        }
        public static Rectangle GetRectangle(System.Windows.FrameworkElement element)
        {
            return new Rectangle(GetRectPosition(element), GetRectSize(element));
        }
        public static Point GetRectSize(System.Windows.FrameworkElement element)
        {
            return new Point()
            {
                X = GetRectWidth(element),
                Y = GetRectHeight(element),
            };
        }
        public static Point GetRectPosition(System.Windows.FrameworkElement element)
        {
            return new Point()
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
        public static Rectangle GetRectangle(System.Windows.Rect rect)
        {
            return new Rectangle((int)rect.TopLeft.X, (int)rect.TopLeft.Y, (int)rect.Width, (int)rect.Height);
        }
        public static Microsoft.Xna.Framework.Color GetColor(Brush brush)
        {
            if (!IsSolidColor(brush))
                return Microsoft.Xna.Framework.Color.White;
            
            return new Microsoft.Xna.Framework.Color()
            {
                R = ((System.Windows.Media.Color)brush.GetValue(SolidColorBrush.ColorProperty)).R,
                G = ((System.Windows.Media.Color)brush.GetValue(SolidColorBrush.ColorProperty)).G,
                B = ((System.Windows.Media.Color)brush.GetValue(SolidColorBrush.ColorProperty)).B,
                A = ((System.Windows.Media.Color)brush.GetValue(SolidColorBrush.ColorProperty)).A,
            };
        }
    }
}
