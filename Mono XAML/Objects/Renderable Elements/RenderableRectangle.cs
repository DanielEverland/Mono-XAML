using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoXAML.Objects
{
    public class RenderableRectangle : RenderableElement
    {
        public RenderableRectangle(FrameworkElement element) : base(element) { }

        public Color Color { get { return Utility.GetColor(_rectangle.Fill); } }

        private System.Windows.Shapes.Rectangle _rectangle { get { return (System.Windows.Shapes.Rectangle)Element; } }

        public override void Render()
        {
            XAMLManager.SpriteBatch.Draw(DefaultTextures.White, Rect, Color);
        }
    }
}
