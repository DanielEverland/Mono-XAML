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
            CreateTextureReferences();
        }

        public Texture2D FillTexture { get { return _fillTexture; } set { _fillTexture = value; } }

        public Microsoft.Xna.Framework.Color StrokeColor { get { return Utility.GetColor(_rectangle.Stroke); } }
        public Microsoft.Xna.Framework.Color FillColor { get { return Utility.GetColor(_rectangle.Fill); } }

        private System.Windows.Shapes.Rectangle _rectangle { get { return (System.Windows.Shapes.Rectangle)Element; } }
        private bool HasBrush { get { return _rectangle.Fill != null; } }
        private bool HasStroke { get { return _rectangle.Stroke != null; } }

        private Texture2D _fillTexture;
        private Texture2D _strokeTexture;

        private void CreateTextureReferences()
        {
            if (!HasBrush)
                return;
            
            _fillTexture = Utility.BrushToTexture(_rectangle.Fill);

            if(HasStroke)
                _strokeTexture = Utility.StrokeToTexture((int)_rectangle.StrokeThickness, _rectangle.Stroke);
        }
        protected override void DoRender()
        {
            if (!HasBrush)
                return;

            XAMLManager.SpriteBatch.Draw(_fillTexture, Rect, FillColor);

            if (HasStroke)
                XAMLManager.SpriteBatch.Draw(_strokeTexture, Rect, StrokeColor);
        }
    }
}
