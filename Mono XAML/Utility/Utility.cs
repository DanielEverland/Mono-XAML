using System;
using System.Windows.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoXAML
{
    public static class Utility
    {
        private const int GRADIENT_RESOLUTION = 128;

        public static bool IsSolidColor(Brush brush)
        {
            return brush is SolidColorBrush;
        }
        public static float Lerp(float value, float a, float b)
        {
            return a * value + b * (1 - value);
        }
        public static Texture2D StrokeToTexture(int strokeThickness, Brush brush)
        {
            Microsoft.Xna.Framework.Color[] colors = new Microsoft.Xna.Framework.Color[GRADIENT_RESOLUTION * GRADIENT_RESOLUTION];

            Texture2D sourceTexture = BrushToTexture(brush);
            Microsoft.Xna.Framework.Color[] sourceColors = new Microsoft.Xna.Framework.Color[sourceTexture.Width * sourceTexture.Height];
            
            sourceTexture.GetData(sourceColors);

            for (int i = 0; i < colors.Length; i++)
            {
                int x = i % GRADIENT_RESOLUTION;
                int y = (int)Math.Floor((double)i / (double)GRADIENT_RESOLUTION);

                if (ShouldRenderStroke(x, y, strokeThickness, GRADIENT_RESOLUTION, GRADIENT_RESOLUTION))
                {
                    colors[i] = GetColor(x, y, sourceColors);
                }
            }

            Texture2D texture = new Texture2D(XAMLManager.GraphicsDeviceManager.GraphicsDevice, GRADIENT_RESOLUTION, GRADIENT_RESOLUTION);

            texture.SetData(colors);

            return texture;
        }
        private static bool ShouldRenderStroke(int x, int y, int strokeThickness, int width, int height)
        {
            return x < strokeThickness || y < strokeThickness || y > height - strokeThickness || x > width - strokeThickness;
        }
        private static Microsoft.Xna.Framework.Color GetColor(int x, int y, Microsoft.Xna.Framework.Color[] source)
        {
            int i = y * GRADIENT_RESOLUTION + x;

            if(i >= source.Length)
            {
                return Microsoft.Xna.Framework.Color.White;
            }
            else
            {
                return source[i];
            }
        }
        public static Texture2D BrushToTexture(Brush brush)
        {
            if(brush is GradientBrush gradient)
            {
                return GradientToTexture(gradient);
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
        public static Texture2D GradientToTexture(GradientBrush gradient)
        {
            if(gradient is LinearGradientBrush linearGradient)
            {
                return LinearGradientToTexture(linearGradient);
            }
            else if(gradient is RadialGradientBrush radialGradient)
            {
                return RadialGradientToTexture(radialGradient);
            }
            else
            {
                throw new NotImplementedException("Cannot render " + gradient.GetType());
            }
        }
        private static Texture2D RadialGradientToTexture(RadialGradientBrush gradientBrush)
        {
            Texture2D texture = new Texture2D(
                    XAMLManager.GraphicsDeviceManager.GraphicsDevice,
                    GRADIENT_RESOLUTION,
                    GRADIENT_RESOLUTION);

            Vector2 radial = gradientBrush.GradientOrigin.ToVector();

            Microsoft.Xna.Framework.Color[] colors = new Microsoft.Xna.Framework.Color[GRADIENT_RESOLUTION * GRADIENT_RESOLUTION];

            for (int i = 0; i < colors.Length; i++)
            {
                int x = i % GRADIENT_RESOLUTION;
                int y = (int)Math.Floor((double)i / (double)GRADIENT_RESOLUTION);

                Vector2 currentPoint = new Vector2(x, y) / GRADIENT_RESOLUTION;
                Vector2 delta = radial - currentPoint;

                delta.X /= (float)gradientBrush.RadiusX;
                delta.Y /= (float)gradientBrush.RadiusY;

                colors[i] = GetColor(delta.Length(), gradientBrush);
            }

            texture.SetData(colors);

            return texture;
        }
        private static Texture2D LinearGradientToTexture(LinearGradientBrush linearGradient)
        {
            Texture2D texture = new Texture2D(
                    XAMLManager.GraphicsDeviceManager.GraphicsDevice, 
                    GRADIENT_RESOLUTION, 
                    GRADIENT_RESOLUTION);

            Vector2 startPoint = linearGradient.StartPoint.ToVector() * GRADIENT_RESOLUTION;
            Vector2 endPoint = linearGradient.EndPoint.ToVector() * GRADIENT_RESOLUTION;

            float gradientDistance = Vector2.Distance(startPoint, endPoint);
            
            Microsoft.Xna.Framework.Color[] colors = new Microsoft.Xna.Framework.Color[GRADIENT_RESOLUTION * GRADIENT_RESOLUTION];

            for (int i = 0; i < colors.Length; i++)
            {
                int x = i % GRADIENT_RESOLUTION;
                int y = (int)Math.Floor((double)i / (double)GRADIENT_RESOLUTION);

                float perpendicularDistance = PerpendicularDistance(startPoint, endPoint, new Vector2(x, y));

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
