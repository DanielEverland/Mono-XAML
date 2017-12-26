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
            Initialize();
        }

        public Texture2D FillTexture { get { return _fillTexture; } set { _fillTexture = value; } }

        public Microsoft.Xna.Framework.Color FillColor { get { return Utility.GetColor(_rectangle.Fill); } }

        private System.Windows.Shapes.Rectangle _rectangle { get { return (System.Windows.Shapes.Rectangle)Element; } }
        private bool HasBrush { get { return _rectangle.Fill != null; } }

        private Texture2D _fillTexture;

        private void Initialize()
        {
            CreateFill();
            CreateStroke();
        }
        private void CreateFill()
        {
            if (!HasBrush)
                return;

            _fillTexture = Utility.BrushToTexture(_rectangle.Fill, WorldSize);
        }
        private void CreateStroke()
        {
            Stroke stroke = new Stroke();

            stroke.SetParent(this);
        }
        protected override void DoRender()
        {
            if (!HasBrush)
                return;

            XAMLManager.SpriteBatch.Draw(_fillTexture, Rect, FillColor);
        }
    }
}
