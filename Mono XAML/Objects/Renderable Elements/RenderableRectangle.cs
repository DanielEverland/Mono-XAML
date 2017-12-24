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
            Texture2D texture = new Texture2D(XAMLManager.GraphicsDeviceManager.GraphicsDevice, 1, 1);
            Color[] color = new Color[1]
            {
                Color.White,
            };

            texture.SetData(color);

            XAMLManager.SpriteBatch.Draw(texture, Rect, Color);
        }
    }
}
