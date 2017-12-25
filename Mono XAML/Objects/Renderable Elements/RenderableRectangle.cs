using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Media;

namespace MonoXAML.Objects
{
    public class RenderableRectangle : RenderableElement
    {
        public RenderableRectangle(FrameworkElement element) : base(element)
        {
            CreateTextureReference();
        }

        public Microsoft.Xna.Framework.Color Color { get { return Utility.GetColor(_rectangle.Fill); } }

        private System.Windows.Shapes.Rectangle _rectangle { get { return (System.Windows.Shapes.Rectangle)Element; } }
        private bool HasBrush { get { return _rectangle.Fill != null; } }
        private Texture2D _texture;

        private void CreateTextureReference()
        {
            if (!HasBrush)
                return;

            if(_rectangle.Fill is LinearGradientBrush linearBrush)
            {
                _texture = Utility.GradientToTexture(linearBrush);
            }
            else if(_rectangle.Fill is RadialGradientBrush radialBrush)
            {
                _texture = Utility.GradientToTexture(radialBrush);
            }
            else if(_rectangle.Fill is SolidColorBrush solidColorBrush)
            {
                _texture = DefaultTextures.White;
            }
            else
            {
                throw new System.ArgumentException("Cannot render " + _rectangle.Fill.GetType());
            }
        }
        public override void Render()
        {
            if (!HasBrush)
                return;

            XAMLManager.SpriteBatch.Draw(_texture, Rect, Color);
        }
    }
}
