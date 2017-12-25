using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonoXAML
{
    public static class Extensions
    {
        public static void Save(this Microsoft.Xna.Framework.Graphics.Texture2D texture, string fileName)
        {
            texture.SaveAsPng(new FileStream(fileName, FileMode.Create, FileAccess.Write), texture.Width, texture.Height);
        }
        public static System.Windows.Media.Color Convert(this Microsoft.Xna.Framework.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        public static Microsoft.Xna.Framework.Color Convert(this System.Windows.Media.Color color)
        {
            return new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A);
        }
        public static Microsoft.Xna.Framework.Vector2 ToVector(this Microsoft.Xna.Framework.Point point)
        {
            return new Microsoft.Xna.Framework.Vector2()
            {
                X = (float)point.X,
                Y = (float)point.Y
            };
        }
        public static Microsoft.Xna.Framework.Vector2 ToVector(this System.Windows.Point point)
        {
            return new Microsoft.Xna.Framework.Vector2()
            {
                X = (float)point.X,
                Y = (float)point.Y
            };
        }
        public static Microsoft.Xna.Framework.Vector2 ToVector(this System.Windows.Size size)
        {
            return new Microsoft.Xna.Framework.Vector2()
            {
                X = (float)size.Width,
                Y = (float)size.Height,
            };
        }
    }
}
