using System;
using System.Collections.Generic;
using System.Text;

namespace MonoXAML
{
    public static class Extensions
    {
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
